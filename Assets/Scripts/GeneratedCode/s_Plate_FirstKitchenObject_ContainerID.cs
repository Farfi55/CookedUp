using ThinkEngine.Sensors;
using ThinkEngine.Models;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_Plate_FirstKitchenObject_ContainerID : Sensor
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
			mappingTemplate = "s_Plate_FirstKitchenObject_ContainerID(plateSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

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
				KOContainerSensorData KOContainerSensorData0 = gameObject.GetComponent<KOContainerSensorData>();
				KitchenObjectASP FirstKitchenObject1 = KOContainerSensorData0.FirstKitchenObject;
				int ContainerID2 = FirstKitchenObject1.ContainerID;
				if (values.Count == 200)
				{
						values.RemoveAt(0);
				}
				values.Add(ContainerID2);
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
