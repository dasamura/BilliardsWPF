using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Prac2
{
    public partial class MainWindow : Window
    {
        private List<ModelVisual3D> _models;

        public MainWindow()
        {
            InitializeComponent();

            _models = new List<ModelVisual3D>();

            // Load 3D models from OBJ files
            LoadModels();

            // Create and add camera to the scene
            AddCamera();

            // Bind models to DataContext
            DataContext = this;

            // Animation
            AnimateModels();
        }

        private void LoadModels()
        {
            var reader1 = new ObjReader();
            var model3D1 = reader1.Read(@"C:\Users\user\source\repos\Prac2\Prac2\obj\tableBilliard.obj");
            var modelVisual3D1 = CreateModelVisual3D(model3D1);
            _models.Add(modelVisual3D1);

            var reader2 = new ObjReader();
            var model3D2 = reader2.Read(@"C:\Users\user\source\repos\Prac2\Prac2\obj\kiy.obj");
            var modelVisual3D2 = CreateModelVisual3D(model3D2);
            _models.Add(modelVisual3D2);

            var reader3 = new ObjReader();
            var model3D3 = reader3.Read(@"C:\Users\user\source\repos\Prac2\Prac2\obj\ball.obj");
            var modelVisual3D3 = CreateModelVisual3D(model3D3);
            _models.Add(modelVisual3D3);

            foreach (var model in _models)
            {
                helixViewport.Children.Add(model);
            }
        }

        private ModelVisual3D CreateModelVisual3D(Model3D model, DirectionalLight light = null)
        {
            var modelVisual3D = new ModelVisual3D();
            var modelGroup = new Model3DGroup();
            modelGroup.Children.Add(model);
            if (light != null)
            {
                modelGroup.Children.Add(light);
            }
            modelVisual3D.Content = modelGroup;
            return modelVisual3D;
        }

        private void AddCamera()
        {
            var camera = new PerspectiveCamera();
            camera.Position = new Point3D(0, 2.4, 5);
            camera.LookDirection = new Vector3D(0, -0.3, -1);
            camera.UpDirection = new Vector3D(0, 1, 3);
            helixViewport.Camera = camera;
        }

        private void AddDirectionalLight()
        {
            var light = new DirectionalLight(Colors.Gray, new Vector3D(1, -1, -1));
            helixViewport.Children.Add(new ModelVisual3D { Content = light }); // Wrap the light in a ModelVisual3D
        }


        private async void AnimateModels()
        {
            for (int i = 0; i < _models.Count; i++)
            {
                AddDirectionalLight(); // Add directional light for each model
                AnimateModel(_models[i]);
                await Task.Delay(2000); // Delay between animations (2 seconds)
            }
        }

        private void AnimateModel(ModelVisual3D model)
        {
            var transform = new TranslateTransform3D();
            var animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 0.4;
            animation.Duration = TimeSpan.FromSeconds(2);


            animation.Completed += (sender, e) =>
            {
                transform.OffsetZ = 0;
            };

            model.Transform = transform;
            transform.BeginAnimation(TranslateTransform3D.OffsetZProperty, animation);
        }
    }
}
