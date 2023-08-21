using System.Collections.Generic;
using System;
using CookedUp.ThinkEngine.Models;
using CookedUp.ThinkEngine.Sensors;
using ThinkEngine.Mappers;
using static ThinkEngine.Mappers.OperationContainer;


namespace ThinkEngine
{
    class s_Const_CookingRecipe_IsBurningRecipe : Sensor
    {
		private int counter;
        private object specificValue;
        private Operation operation;
		private BasicTypeMapper mapper;
		private List<List<bool>> values = new List<List<bool>>();


		public override void Initialize(SensorConfiguration sensorConfiguration)
		{
			this.gameObject = sensorConfiguration.gameObject;
			ready = true;
			int index = gameObject.GetInstanceID();
			mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof(bool));
			operation = mapper.OperationList()[0];
			counter = 0;
			mappingTemplate = "s_Const_CookingRecipe_IsBurningRecipe(constantsSensor,objectIndex("+index+"),{1},{0})." + Environment.NewLine;

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
				ConstantsSensorData ConstantsSensorData0 = gameObject.GetComponent<ConstantsSensorData>();
				List<CookingRecipe> CookingRecipes1 = ConstantsSensorData0.CookingRecipes;
				if(CookingRecipes1 == null)
				{
					values.Clear();
					return;
				}
				else if(CookingRecipes1.Count > values.Count)
				{
					for(int i = values.Count; i < CookingRecipes1.Count; i++)
					{
						values.Add(new List<bool>());
					}
				}
				else if(CookingRecipes1.Count < values.Count)
				{
					for(int i = CookingRecipes1.Count; i < values.Count; i++)
					{
						values.RemoveAt(values.Count - 1);
					}
				}
				for(int i_1 = 0; i_1 < CookingRecipes1.Count; i_1++)
				{
					if(CookingRecipes1[i_1] == null)
					{
						values[i_1].Clear();
						continue;
					}
					bool IsBurningRecipe2 = CookingRecipes1[i_1].IsBurningRecipe;
					if (values[i_1].Count == 200)
					{
							values[i_1].RemoveAt(0);
					}
					values[i_1].Add(IsBurningRecipe2);
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
