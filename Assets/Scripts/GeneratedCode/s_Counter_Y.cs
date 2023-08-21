using System;
using System.Collections.Generic;
using CookedUp.ThinkEngine.Sensors;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_Counter_Y : Sensor
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
			mappingTemplate = "s_Counter_Y(counterSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

		}

		public override void Destroy()
		{
		}

		public override void Update()
		{
            try
            {
			if(!ready)
			{
				return;
			}
			if(!invariant || first)
			{
				first = false;
				GridPositionSensorData GridPositionSensorData0 = gameObject.GetComponent<GridPositionSensorData>();
				int Y1 = GridPositionSensorData0.Y;
				if (values.Count == 200)
				{
						values.RemoveAt(0);
				}
				values.Add(Y1);
			}
            }
            catch (NullReferenceException nullEx)
            {
                UnityEngine.Debug.LogError(nullEx.Message);
                values.Clear();
                return;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
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
