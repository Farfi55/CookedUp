using System;
using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Mappers;
using ThinkEngine.Models;
using ThinkEngine.Sensors.Counters;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    class S_StoveCounter11 : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<bool> values = new List<bool>();

		/*
		//Singleton
        protected static Sensor instance = null;

        internal static Sensor Instance
        {
            get
            {
                if (instance == null)
                {
					instance = new S_StoveCounter11();
                }
                return instance;
            }
        }*/

		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
            // Debug.Log("Initialize method called!");
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(bool));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_StoveCounter_CurrentCookingRecipe_IsBurningRecipe(counterSensor,objectIndex(1),{0})." + Environment.NewLine;

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
				StoveCounterSensorData StoveCounterSensorData0 = gameObject.GetComponent<StoveCounterSensorData>();
				if(StoveCounterSensorData0 == null) return;
				CookingRecipe CurrentCookingRecipe1 = StoveCounterSensorData0.CurrentCookingRecipe;
				if(CurrentCookingRecipe1 == null) return;
				bool IsBurningRecipe2 = CurrentCookingRecipe1.IsBurningRecipe;

				if (values.Count == 200)
				{
					values.RemoveAt(0);
				}
				values.Add(IsBurningRecipe2);
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