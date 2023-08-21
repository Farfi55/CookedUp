using System;
using System.Collections.Generic;
using CookedUp.ThinkEngine.Sensors.Counters;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_ContainerCounter_KOType : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<string> values = new List<string>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(string));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_ContainerCounter_KOType(counterSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

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
				ContainerCounterSensorData ContainerCounterSensorData0 = gameObject.GetComponent<ContainerCounterSensorData>();
				string KOType1 = ContainerCounterSensorData0.KOType;
				if(KOType1 == null)
				{
					values.Clear();
					return;
				}
				else
				{
					if (values.Count == 200)
					{
							values.RemoveAt(0);
					}
					values.Add(KOType1);
				}
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
