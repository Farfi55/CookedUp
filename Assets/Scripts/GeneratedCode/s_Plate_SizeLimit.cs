using System;
using System.Collections.Generic;
using CookedUp.ThinkEngine.Sensors;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_Plate_SizeLimit : Sensor
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
			mappingTemplate = "s_Plate_SizeLimit(kitchenObjectSensor,objectIndex("+index+"),{0})." + Environment.NewLine;

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
				KOContainerSensorData KOContainerSensorData0 = gameObject.GetComponent<KOContainerSensorData>();
				int SizeLimit1 = KOContainerSensorData0.SizeLimit;
				if (values.Count == 200)
				{
						values.RemoveAt(0);
				}
				values.Add(SizeLimit1);
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
