using System;
using System.Collections.Generic;
using CookedUp.ThinkEngine.Sensors.Counters;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_CuttingCounter_TimeRemainingToCut : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<int> values = new List<int>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(int));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_CuttingCounter_TimeRemainingToCut(cuttingCounterSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

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
				CuttingCounterSensorData CuttingCounterSensorData0 = gameObject.GetComponent<CuttingCounterSensorData>();
				int TimeRemainingToCut1 = CuttingCounterSensorData0.TimeRemainingToCut;
				if (values.Count == 200)
				{
						values.RemoveAt(0);
				}
				values.Add(TimeRemainingToCut1);
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
