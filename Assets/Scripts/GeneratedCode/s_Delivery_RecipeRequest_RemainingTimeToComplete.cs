using System.Collections.Generic;
using System;
using CookedUp.ThinkEngine.Models;
using CookedUp.ThinkEngine.Sensors;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_Delivery_RecipeRequest_RemainingTimeToComplete : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<List<int>> values = new List<List<int>>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(int));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_Delivery_RecipeRequest_RemainingTimeToComplete(deliverySensor,objectIndex("+index+"),{1},{0})." + Environment.NewLine;

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
				List<RecipeRequestASP> WaitingRecipesRequests1 = DeliverySensorData0.WaitingRecipesRequests;
				if(WaitingRecipesRequests1 == null)
				{
					values.Clear();
					return;
				}
				else if(WaitingRecipesRequests1.Count > values.Count)
				{
					for(int i = values.Count; i < WaitingRecipesRequests1.Count; i++)
					{
						values.Add(new List<int>());
					}
				}
				else if(WaitingRecipesRequests1.Count < values.Count)
				{
					for(int i = WaitingRecipesRequests1.Count; i < values.Count; i++)
					{
						values.RemoveAt(values.Count - 1);
					}
				}
				for(int i_1 = 0; i_1 < WaitingRecipesRequests1.Count; i_1++)
				{
					if(WaitingRecipesRequests1[i_1] == null)
					{
						values[i_1].Clear();
						continue;
					}
					int RemainingTimeToComplete2 = WaitingRecipesRequests1[i_1].RemainingTimeToComplete;
					if (values[i_1].Count == 200)
					{
							values[i_1].RemoveAt(0);
					}
					values[i_1].Add(RemainingTimeToComplete2);
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
