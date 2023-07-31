using System;
using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Mappers;
using ThinkEngine.Models;
using ThinkEngine.Sensors.Counters;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    class S_StoveCounter7 : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<string> values = new List<string>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
            // Debug.Log("Initialize method called!");
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(string));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_StoveCounter_CurrentCookingRecipe_OutputKOName(counterSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

		}

		public override void Destroy()
		{
		}

		public override void Update()
		{
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
				string OutputKOName2 = CurrentCookingRecipe1.OutputKOName;
				if(OutputKOName2 == null) return;

				if (values.Count == 200)
				{
					values.RemoveAt(0);
				}
				values.Add(OutputKOName2);
			}
		}

		public override string Map()
		{
			object operationResult = operation(values, specificValue, counter);
			if(operationResult != null)
			{
				return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));
			}
			else
			{
				return "";
			}
		}
    }
}
