using System;
using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Mappers;
using ThinkEngine.Models;
using ThinkEngine.Sensors;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    class S_Counter8 : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<string> values = new List<string>();

		/*
		//Singleton
        protected static Sensor instance = null;

        internal static Sensor Instance
        {
            get
            {
                if (instance == null)
                {
					instance = new S_Counter8();
                }
                return instance;
            }
        }*/

		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
            // Debug.Log("Initialize method called!");
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(string));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_Counter_FirstKitchenObject_Name(counterSensor,objectIndex(1),{0})." + Environment.NewLine;

		}

		public override void Destroy()
		{
            // Debug.Log("Destroy method called!");
			//instance = null;
		}

		public override void Update()
		{
            // Debug.Log("Update method called!");
			if(!ready)
			{
				return;
			}
			if(!invariant || first)
			{
				first = false;
				KOContainerSensorData KOContainerSensorData0 = gameObject.GetComponent<KOContainerSensorData>();
				if(KOContainerSensorData0 == null) return;
				KitchenObject FirstKitchenObject1 = KOContainerSensorData0.FirstKitchenObject;
				if(FirstKitchenObject1 == null) return;
				string Name2 = FirstKitchenObject1.Name;
				if(Name2 == null) return;

				if (values.Count == 200)
				{
					values.RemoveAt(0);
				}
				values.Add(Name2);
			}
		}

		public override string Map()
		{
            // Debug.Log("Map method called!");
			object operationResult = operation(values, specificValue, counter);
			return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));
		}
    }
}