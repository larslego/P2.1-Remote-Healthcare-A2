﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVREngine
{
    class BasicScene
    {
        private List<Func<string>> CommandList;
        private TunnelHandler Handler;

        private string uuidRoute;
        private string uuidModel;

        public BasicScene(TunnelHandler Handler)
        {
            this.CommandList = new List<Func<string>>();
            this.Handler = Handler;

            // Add methods to queue.
            this.CommandList.Add(CreateTerrain);
            this.CommandList.Add(RemoveGroundPlane);
            this.CommandList.Add(ChangeTime);
            this.CommandList.Add(AddModels);
            this.CommandList.Add(AddRoute);
            this.CommandList.Add(AddRoad);
            this.CommandList.Add(MoveModelOverRoad);
        }

        /// <summary>
        /// This method is called and will execute the next step in the exercise.
        /// </summary>
        public string ExecuteNext(int index)
        {
            if (index < this.CommandList.Count) {
                return this.CommandList[index].Invoke();
            } else
            {
                return "There is nothing left to do.";
            }
        }

        /// <summary>
        /// Step 1. Create a new terrain with size: 256 x 256.
        /// </summary>
        private string CreateTerrain()
        {
            double[] height = new double[256*256];

            for (int i = 0; i < height.Length; i++)
            {
                height[i] = i / 100000;
            }

            this.Handler.SendToTunnel(JSONCommandHelper.WrapTerrain(new int[] { 256, 256 }, height));
            this.Handler.SendToTunnel(JSONCommandHelper.WrapShowTerrain("ground", new Transform(1, new int[3] { -128, 0, -128 }, new int[3] { 0, 0, 0 })));

            this.Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("raceterrain", "data/NetworkEngine/models/podracemap1/podracemap1.obj", new Transform(1, new int[3] { 0, 0, 0 }, new int[3] { 0, 0, 0 })));

            return "Created a new terrain with size: 256 x 256.";
        }

        /// <summary>
        /// Step 2. Remove the terrain.
        /// </summary>
        public string RemoveGroundPlane()
        {
            this.Handler.SendToTunnel(JSONCommandHelper.GetAllNodes(), new Action<string>(RemoveGroundPlaneResult));

            return "Removed the terrain.";
        }

        public void RemoveGroundPlaneResult(string jsonString)
        {
            JObject jObject = JObject.Parse(jsonString);
            JArray array = (JArray)jObject.SelectToken("data.data.data.children");

            foreach (JObject o in array)
            {
                Console.WriteLine(o.GetValue("name"));
                if (o.GetValue("name").ToString() == "GroundPlane")
                {
                    this.Handler.SendToTunnel(JSONCommandHelper.RemoveNode(o.GetValue("uuid").ToString()));
                    return;
                }
            }
        }

        /// <summary>
        /// Step 3. Change the time to 5:30.
        /// </summary>
        private string ChangeTime()
        {
            this.Handler.SendToTunnel(JSONCommandHelper.WrapTime(5.5));
            return "Changed the time.";
        }

        /// <summary>
        /// Step 4. Place a new house.
        /// </summary>
        private string AddModels()
        {
            this.Handler.SendToTunnel(JSONCommandHelper.WrapTime(14.5));
            this.Handler.SendToTunnel(JSONCommandHelper.Wrap3DObject("podracer", "data/NetworkEngine/models/podracer/podracer.obj", new Transform(1 , new int[3] { 0, 0, 0}, new int[3] { 0, 0, 0 })));
            return "Spawned a podracer.";
        }


        /// <summary>
        /// Step 6. Create a new route.
        /// </summary>
        private string AddRoute()
        {

            PosVector[] posVectors = new PosVector[] 
            {
            new PosVector(new int[]{0,0,0 }, new int[]{0,0,0}),
            new PosVector(new int[]{4,0,0 }, new int[]{0,0,0}),
            new PosVector(new int[]{4,0,4 }, new int[]{0,0,0}),
            new PosVector(new int[]{0,0,4 }, new int[]{0,0,0}),
        };
            
            this.Handler.SendToTunnel(JSONCommandHelper.WrapAddRoute(posVectors));
            return "Added a route.";
        }

        /// <summary>
        /// Step 7. Add a road to the previous route.
        /// </summary>
        private string AddRoad()
        { 
            //Handler.SendToTunnel(JSONCommandHelper.WrapAddRouteTerrain(uuidRoute));
            return "Added a road to the previous route.";
        }

        /// <summary>
        /// Step 8. Move a model over the route.
        /// </summary>
        private string MoveModelOverRoad()
        {
            //Handler.SendToTunnel(JSONCommandHelper.WrapFollow(uuidRoute,uuidModel));
            return "The bike is now moving over the route.";
        }
    }
}
