# RobotChef

Made with the intention of helping with teaching and showing simple coding concepts to kids. The app is a simple game where the user can create a robot chef and teach it how to cook different recipes.

## Requirements

- .NET

## How to use

### Getting a recipe

```csharp
Recipe recipe = TakeOrder("Salad");
```
or
```csharp
Recipe recipe = TakeOrder(RecipeNames.Salad);
```

### Making a salad
```csharp
Ingredient salat = Get(IngredientNames.Salat);
Ingredient tomato = Get(IngredientNames.Tomato);
Ingredient cucumber = Get(IngredientNames.Cucumber);

Mix saladMix = Mix("Salad Mix", [salat, tomato, cucumber]);

Wash(ref saladMix);
for (int i = 0; i < 5; i++)
{
    Cut(ref saladMix);
}

Food salad = Prepare("Salad", saladMix, []);
```

### Making a burger
```csharp
Ingredient bun = Get("Bun", true);
if (bun.IsFrozen) Heat(ref bun);

Ingredient meat = Get("Meat", true);
Heat(ref meat);

Ingredient salat = Get("Salat");
Wash(ref salat);
for (int i = 0; i < 5; i++)
    Cut(ref salat);

Ingredient tomato = Get("Tomato");
Wash(ref tomato);
for (int i = 0; i < 3; i++)
    Cut(ref tomato);

Ingredient cucumber = Get("Cucumber");
Wash(ref cucumber);
for (int i = 0; i < 3; i++)
    Cut(ref cucumber);

Ingredient ketchup = Get("Ketchup");

Ingredient[] ingredients = [bun, meat, salat, tomato, cucumber, ketchup, bun];
Food burger = Prepare("Burger", ingredients);
```

### Delivering and Calculating change
```csharp
int cash = Deliver(burger, recipe);

if (cash > recipe.Price)
{
    int change = CalculateChange(cash, recipe.Price);
    GiveChange(change);
}
```

### Example from getting recipe to delivering a burger w/change
```csharp
Recipe recipe = TakeOrder(RecipeNames.Burger);

Ingredient bunBottom = Get(IngredientNames.Bun, true);
if (bunBottom.IsFrozen) Heat(ref bunBottom);
Ingredient bunTop = Get(IngredientNames.Bun, true);
if (bunTop.IsFrozen) Heat(ref bunTop);

Ingredient meat = Get(IngredientNames.Meat, true);
Heat(ref meat);

Ingredient salat = Get(IngredientNames.Salat);
Wash(ref salat);
for (int i = 0; i < 5; i++)
    Cut(ref salat);

Ingredient tomato = Get(IngredientNames.Tomato);
Wash(ref tomato);
for (int i = 0; i < 3; i++)
    Cut(ref tomato);

Ingredient cucumber = Get(IngredientNames.Cucumber);
Wash(ref cucumber);
for (int i = 0; i < 3; i++)
    Cut(ref cucumber);

Ingredient ketchup = Get(IngredientNames.Ketchup);

Ingredient[] ingredients = [bunBottom, meat, salat, tomato, cucumber, ketchup, bunTop];
Food burger = Prepare(RecipeNames.Burger, ingredients);

int cash = Deliver(burger, recipe);

if (cash > recipe.Price)
{
    int change = CalculateChange(cash, recipe.Price);
    GiveChange(change);
}
```