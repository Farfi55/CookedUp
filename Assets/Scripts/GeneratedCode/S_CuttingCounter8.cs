using System;
using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Mappers;
using ThinkEngine.Sensors;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    class S_CuttingCounter8 : Sensor
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
					instance = new S_CuttingCounter8();
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
			mappingTemplate = "s_CuttingCounter_Type(cuttingCounterSensor,objectIndex(1),{0})." + Environment.NewLine;

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
				BaseSensorData BaseSensorData0 = gameObject.GetComponent<BaseSensorData>();
				if(BaseSensorData0 == null) return;
				string Type1 = BaseSensorData0.Type;
				if(Type1 == null) return;

				if (values.Count == 200)
				{
					values.RemoveAt(0);
				}
				values.Add(Type1);
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