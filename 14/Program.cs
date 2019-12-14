using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _14
{
    class Recipe
    {
        public List<Ingredient> InputIngredients;
        public Ingredient OutputIngredient;

        public Recipe(List<Ingredient> inputIngredients, Ingredient outputIngredient)
        {
            this.InputIngredients = inputIngredients;
            this.OutputIngredient = outputIngredient;
        }
    }

    class Ingredient
    {
        public string Chemical;
        public Int64 Quantity;

        public Ingredient(string chemical, Int64 quantity)
        {
            this.Chemical = chemical;
            this.Quantity = quantity;
        }
    }

    class Program
    {
        static Dictionary<string, Recipe> Reactions = new Dictionary<string, Recipe>();

        static Dictionary<string, Int64> ProducedChemicals = new Dictionary<string, Int64>();

        static Int64 OreProduced = 0;

        static Ingredient StringToIngredient(string str)
        {
            string[] quantityNameSplit = str.Trim().Split(' ');
            Int64 quantity = Int32.Parse(quantityNameSplit[0]);
            string chemical = quantityNameSplit[1];
            return new Ingredient(chemical, quantity);
        }
        static Recipe StringToRecipe(string str)
        {
            string[] inOutSplit = str.Split('=');
            inOutSplit[0] = inOutSplit[0];
            inOutSplit[1] = inOutSplit[1].Substring(1);

            string[] inputsSplit = inOutSplit[0].Split(',');
            List<Ingredient> inputIngredients = new List<Ingredient>();
            foreach (string input in inputsSplit)
            {
                inputIngredients.Add(StringToIngredient(input));
            }

            Ingredient outputIngredient = StringToIngredient(inOutSplit[1]);

            return new Recipe(inputIngredients, outputIngredient);
        }

        static void Produce(Recipe recipe, Int64 quantity)
        {
            ProducedChemicals[recipe.OutputIngredient.Chemical] -= quantity;
            Int64 howManyLeftToProduce = -ProducedChemicals[recipe.OutputIngredient.Chemical];
            if (howManyLeftToProduce > 0)
            {
                Int64 howManyEachRecipeProduce = recipe.OutputIngredient.Quantity;
                Int64 howManyRecipes = (Int64)Math.Ceiling((double)howManyLeftToProduce / howManyEachRecipeProduce);
                Int64 howManyProduced = howManyRecipes * howManyEachRecipeProduce;
                Int64 howManySpare = howManyProduced - howManyLeftToProduce;
                ProducedChemicals[recipe.OutputIngredient.Chemical] += howManyProduced;

                //for (Int64 i = 0; i < howManyRecipes; i++)
                //{
                foreach (Ingredient inputIngredident in recipe.InputIngredients)
                {
                    if (inputIngredident.Chemical == "ORE")
                    {
                        Int64 oreIncrement = inputIngredident.Quantity * howManyRecipes;
                        OreProduced = OreProduced + oreIncrement;
                    }
                    else
                    {
                        Produce(Reactions[inputIngredident.Chemical], inputIngredident.Quantity * howManyRecipes);
                    }
                }
                //}
            }
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            foreach (string recipe in inputList)
            {
                Recipe newRecipe = StringToRecipe(recipe);
                Reactions[newRecipe.OutputIngredient.Chemical] = newRecipe;
                ProducedChemicals[newRecipe.OutputIngredient.Chemical] = 0;
            }

            Produce(Reactions["FUEL"], 1);
            Console.WriteLine("1 FUEL requires {0} ORE", OreProduced);

            Int64 fuelRequired = 3060000; // rough start point (could implement binary switch)
            while (OreProduced <= 1000000000000)
            {
                fuelRequired++;
                OreProduced = 0;
                foreach (var key in ProducedChemicals.Keys.ToList())
                {
                    ProducedChemicals[key] = 0;
                }

                if (fuelRequired % 1000 == 0)
                    Console.WriteLine(fuelRequired);

                Produce(Reactions["FUEL"], fuelRequired);
            }

            Console.WriteLine("1 Trillion ORE produces {0} FUEL", fuelRequired-1) ;

            Console.ReadLine();
        }
    }
}
