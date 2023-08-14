c_KO_Name("Bread").
c_KO_Name("Cabbage").
c_KO_Name("CabbageSlices").
c_KO_Name("CheeseBlock").
c_KO_Name("CheeseSlices").
c_KO_Name("MeatPattyBurned").
c_KO_Name("MeatPattyCooked").
c_KO_Name("MeatPattyUncooked").
c_KO_Name("Plate").
c_KO_Name("Tomato").
c_KO_Name("TomatoSlices").


c_CompleteRecipe("Burger", 10).
c_CompleteRecipe("CheeseBurger", 15).
c_CompleteRecipe("MegaBurger", 30).
c_CompleteRecipe("Salad", 8).

c_CompleteRecipe_Ingredient("Burger", "Bread").
c_CompleteRecipe_Ingredient("Burger", "MeatPattyCooked").

c_CompleteRecipe_Ingredient("CheeseBurger", "Bread").
c_CompleteRecipe_Ingredient("CheeseBurger", "MeatPattyCooked").
c_CompleteRecipe_Ingredient("CheeseBurger", "CheeseSlices").

c_CompleteRecipe_Ingredient("MegaBurger", "Bread").
c_CompleteRecipe_Ingredient("MegaBurger", "MeatPattyCooked").
c_CompleteRecipe_Ingredient("MegaBurger", "CheeseSlices").
c_CompleteRecipe_Ingredient("MegaBurger", "TomatoSlices").
c_CompleteRecipe_Ingredient("MegaBurger", "CabbageSlices").

c_CompleteRecipe_Ingredient("Salad", "TomatoSlices").
c_CompleteRecipe_Ingredient("Salad", "CabbageSlices").


c_CookingRecipe(
    "MeatPattyUncooked-MeatPattyCooked", 
    "MeatPattyUncooked", "MeatPattyCooked", 
    5000, false).

c_CookingRecipe(
    "MeatPattyCooked-MeatPattyBurned", 
    "MeatPattyCooked", "MeatPattyBurned", 
    3500, true).


c_CuttingRecipe_Name(
    "Cabbage-CabbageSlices", 
    "Cabbage", "CabbageSlices", 
    2000).

c_CuttingRecipe_Name(
    "Tomato-TomatoSlices", 
    "Tomato", "TomatoSlices", 
    1000).

c_CuttingRecipe_Name(
    "CheeseBlock-CheeseSlices", 
    "CheeseBlock", "CheeseSlices", 
    1500).
