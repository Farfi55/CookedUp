using ThinkEngine.Sensors;
using System.Collections.Generic;
using System;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_Delivery_WaitingRecipesNames : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<List<string>> values = new List<List<string>>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(string));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_Delivery_WaitingRecipesNames(deliverySensor,objectIndex("+index+"),{1},{0})." + Environment.NewLine;

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
				DeliverySensorData DeliverySensorData0 = gameObject.GetComponent<DeliverySensorData>();
				List<string> WaitingRecipesNames1 = DeliverySensorData0.WaitingRecipesNames;
				if(WaitingRecipesNames1 == null)
				{
					values.Clear();
					return;
				}
				else if(WaitingRecipesNames1.Count > values.Count)
				{
					for(int i = values.Count; i < WaitingRecipesNames1.Count; i++)
					{
						values.Add(new List<string>());
					}
				}
				else if(WaitingRecipesNames1.Count < values.Count)
				{
					for(int i = WaitingRecipesNames1.Count; i < values.Count; i++)
					{
						values.RemoveAt(values.Count - 1);
					}
				}
				for(int i_1 = 0; i_1 < WaitingRecipesNames1.Count; i_1++)
				{
					string WaitingRecipesNames2 = WaitingRecipesNames1[i_1];
					if(WaitingRecipesNames2 == null)
					{
						values[i_1].Clear();
						continue;
					}
					else
					{
						if (values[i_1].Count == 200)
						{
								values[i_1].RemoveAt(0);
						}
						values[i_1].Add(WaitingRecipesNames2);
					}
				}
			}
		}

		public override string Map()
		{
			string mapping = string.Empty;
			for( int i0=0; i0<values.Count;i0++)
			{
				object operationResult = operation(values[i0], specificValue, counter);
				if(operationResult != null)
				{
					mapping = string.Concat(mapping, string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult),i0));
				}
				else
				{
					mapping = string.Concat(mapping, string.Format("{0}", Environment.NewLine));
				}
			}
			return mapping;

		}
    }
}
