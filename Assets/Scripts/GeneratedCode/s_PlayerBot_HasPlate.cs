using ThinkEngine.Sensors;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_PlayerBot_HasPlate : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<bool> values = new List<bool>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(bool));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_PlayerBot_HasPlate(player,objectIndex("+index+"),{0})." + Environment.NewLine;

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
				PlayerBotSensorData PlayerBotSensorData0 = gameObject.GetComponent<PlayerBotSensorData>();
				bool HasPlate1 = PlayerBotSensorData0.HasPlate;
				if (values.Count == 200)
				{
						values.RemoveAt(0);
				}
				values.Add(HasPlate1);
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
