using System;

namespace NeuralCars
{
    public class Car
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Angle { get; set; }  //kąt obrotu w radianach
        public double Speed { get; set; } 

        public bool IsDead { get; set; } = false;
        public double Fitness { get; set; } = 0;  //licznik punktów

        public NeuralNetwork Brain;  //decyduje o ruch

        private const double MapWidth = 800;
        private const double MapHeight = 600;

        private double lastX;
        private double lastY;

        public Car()
        {
            Reset();
            Brain = new NeuralNetwork(4, 2); 
        }

        public Car(NeuralNetwork parentBrain) //kopiuje mozg rodzica
        {
            Reset();
            Brain = new NeuralNetwork(parentBrain); 
        }

        public void Reset()
        {
            X = MapWidth / 2;
            Y = MapHeight / 2;
            Angle = -Math.PI / 2; 
            IsDead = false;
            Fitness = 0;
        }

        public void Update()  //wywoływana 60 razy na sek
        {
            if (IsDead) return;

            // Zapamiętujemy pozycję przed ruchem
            lastX = X;
            lastY = Y;

            double dTop = Y / MapHeight;  //od 0.0 do 1.0.
            double dBottom = (MapHeight - Y) / MapHeight;
            double dLeft = X / MapWidth;
            double dRight = (MapWidth - X) / MapWidth;

            double[] inputs = { dTop, dBottom, dLeft, dRight };

            double[] output = Brain.FeedForward(inputs);

            double turn = output[0] * 0.1; // dla płynności rysowania (0.1 radiana = 5.7 stopnia)
            Angle += turn;

            Speed = (output[1] + 1) * 4;

            X += Math.Cos(Angle) * Speed;
            Y += Math.Sin(Angle) * Speed;

            // Obliczamy dystans przebyty w tej klatce
            double distanceMoved = Math.Sqrt(Math.Pow(X - lastX, 2) + Math.Pow(Y - lastY, 2));

            //  Nagradzamy prędkość i ruch do przodu, ale karzemy za bardzo ostre skręcanie
            // (Jeśli turn jest blisko 0, mnożnik jest wysoki. Jeśli auto mocno skręca, dostaje mniej punktów)
            double movementBonus = distanceMoved * (1.0 - Math.Abs(output[0]) * 0.5);

            Fitness += movementBonus;

            if (X < 0 || X > MapWidth || Y < 0 || Y > MapHeight)
            {
                IsDead = true;
            }
        }
    }
}