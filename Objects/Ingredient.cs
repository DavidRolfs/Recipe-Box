using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeBox.Objects
{
  public class Ingredient
  {
    private string _ingredientName;
    private int _id;

    public Ingredient(string ingredientName, int Id = 0)
    {
      _ingredientName = ingredientName;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetIngredientName()
    {
      return _ingredientName;
    }

    public override bool Equals(System.Object otherIngredient)
    {
      if (!(otherIngredient is Ingredient))
      {
        return false;
      }
      else
      {
        Ingredient newIngredient = (Ingredient) otherIngredient;
        bool idEquality = (this.GetId() == newIngredient.GetId());
        bool ingredientNameEquality = (this.GetIngredientName() == newIngredient.GetIngredientName());
        return (idEquality && ingredientNameEquality);
      }
    }

    public static void DeleteAll()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("DELETE FROM ingredients;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
   }

   public static List<Ingredient> GetAll()
    {
      List<Ingredient> allIngredients = new List<Ingredient>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int ingredientId = rdr.GetInt32(0);
        string ingredientName = rdr.GetString(1);
        Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
        allIngredients.Add(newIngredient);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allIngredients;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO ingredients (ingredient_name) OUTPUT INSERTED.id VALUES (@IngredientName)", conn);

      SqlParameter ingredientNameParameter = new SqlParameter();
      ingredientNameParameter.ParameterName = "@IngredientName";
      ingredientNameParameter.Value = this.GetIngredientName();

      cmd.Parameters.Add(ingredientNameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Ingredient Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE id = @IngredientId", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = id.ToString();
      cmd.Parameters.Add(ingredientIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundIngredientId = 0;
      string foundIngredientName = null;

      while(rdr.Read())
      {
        foundIngredientId = rdr.GetInt32(0);
        foundIngredientName = rdr.GetString(1);
      }
      Ingredient foundIngredient = new Ingredient(foundIngredientName, foundIngredientId);

      if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }

     return foundIngredient;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM ingredients WHERE id = @IngredientId; DELETE FROM recipes_ingredients WHERE ingredients_id = @IngredientId;", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = this.GetId();

      cmd.Parameters.Add(ingredientIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddRecipe(Recipe newRecipe)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_ingredients (ingredients_id, recipes_id) VALUES (@IngredientId, @RecipeId);", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = this.GetId();
      cmd.Parameters.Add(ingredientIdParameter);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = newRecipe.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Recipe> GetRecipes()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM ingredients JOIN recipes_ingredients ON (ingredients.id = recipes_ingredients.ingredients_id) JOIN recipes ON (recipes_ingredients.recipes_id = recipes.id) WHERE ingredients.id = @IngredientId;", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = this.GetId();

      cmd.Parameters.Add(ingredientIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Recipe> recipes = new List<Recipe> {};

      while(rdr.Read())
        {
          int thisRecipeId = rdr.GetInt32(0);
          string recipeName = rdr.GetString(1);
          int recipeRating = rdr.GetInt32(2);
          string recipeInstructions = rdr.GetString(3);
          Recipe foundRecipe = new Recipe(recipeName, recipeRating, recipeInstructions, thisRecipeId);
          recipes.Add(foundRecipe);
        }
        if (rdr != null)
        {
          rdr.Close();
        }

      if (conn != null)
      {
        conn.Close();
      }
      return recipes;
    }
    public static List<Ingredient> SearchIngredients(string searchIngredientName)
    {
      List<Ingredient> allIngredients = new List<Ingredient>{};

      SqlConnection conn = DB.Connection();
      conn.Open();


      SqlCommand cmd = new SqlCommand("SELECT * FROM ingredients WHERE ingredient_name LIKE @IngredientName;", conn);

      SqlParameter ingredientNameParam = new SqlParameter();
      ingredientNameParam.ParameterName = "@IngredientName";
      ingredientNameParam.Value = "%" + searchIngredientName + "%";

      cmd.Parameters.Add(ingredientNameParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int ingredientId = rdr.GetInt32(0);
        string ingredientName = rdr.GetString(1);

        Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
        allIngredients.Add(newIngredient);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allIngredients;
    }
    public void DeleteIngredient()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM recipes_ingredients WHERE ingredients_id = @IngredientId;", conn);
      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = this.GetId();

      cmd.Parameters.Add(ingredientIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
