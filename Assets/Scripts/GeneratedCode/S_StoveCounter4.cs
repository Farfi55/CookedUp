using System;
using System.Collections.Generic;
using UnityEngine;
using ThinkEngine.Mappers;
using ThinkEngine.Sensors.Counters;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    class S_StoveCounter4 : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<bool> values = new List<bool>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
            // Debug.Log("Initialize method called!");
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(bool));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_StoveCounter_IsBurning(counterSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

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
				bool IsBurning1 = StoveCounterSensorData0.IsBurning;

				if (values.Count == 200)
				{
					values.RemoveAt(0);
				}
				values.Add(IsBurning1);
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
