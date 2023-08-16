using ThinkEngine.Sensors;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_KO_Name : Sensor
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
			mappingTemplate = "s_KO_Name(kitchenObjectSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

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
				BaseSensorData BaseSensorData0 = gameObject.GetComponent<BaseSensorData>();
				string Name1 = BaseSensorData0.Name;
				if(Name1 == null)
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
					values.Add(Name1);
				}
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
