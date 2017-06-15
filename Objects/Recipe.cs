using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeBox.Objects
{
  public class Recipe
  {
    private string _recipeName;
    private int _rating;
    private string _instructions;
    private int _id;

    public Recipe(string RecipeName, int Rating, string Instructions, int Id = 0)
    {
      _recipeName = RecipeName;
      _rating = Rating;
      _instructions = Instructions;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetRecipeName()
    {
      return _recipeName;
    }
    public int GetRating()
    {
      return _rating;
    }
    public string GetInstructions()
    {
      return _instructions;
    }

    public override bool Equals(System.Object otherRecipe)
    {
      if (!(otherRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherRecipe;
        bool idEquality = (this.GetId() == newRecipe.GetId());
        bool recipeNameEquality = (this.GetRecipeName() == newRecipe.GetRecipeName());
        bool ratingEquality = (this.GetRating() == newRecipe.GetRating());
        bool instructionsEquality = (this.GetInstructions() == newRecipe.GetInstructions());
        return (idEquality && recipeNameEquality && ratingEquality && instructionsEquality);
      }
    }

    public static void DeleteAll()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("DELETE FROM recipes;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
   }

   public static List<Recipe> GetAll()
    {
      List<Recipe> allRecipes = new List<Recipe>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes ORDER BY recipe_name;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        int recipeRating = rdr.GetInt32(2);
        string recipeInstructions = rdr.GetString(3);
        Recipe newRecipe = new Recipe(recipeName, recipeRating, recipeInstructions, recipeId);
        allRecipes.Add(newRecipe);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allRecipes;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes (recipe_name, rating, instructions) OUTPUT INSERTED.id VALUES (@RecipeName, @RecipeRating, @RecipeInstructions)", conn);

      SqlParameter recipeNameParameter = new SqlParameter();
      recipeNameParameter.ParameterName = "@RecipeName";
      recipeNameParameter.Value = this.GetRecipeName();

      SqlParameter recipeRatingParameter = new SqlParameter();
      recipeRatingParameter.ParameterName = "@RecipeRating";
      recipeRatingParameter.Value = this.GetRating();

      SqlParameter recipeInstructionsParameter = new SqlParameter();
      recipeInstructionsParameter.ParameterName = "@RecipeInstructions";
      recipeInstructionsParameter.Value = this.GetInstructions();

      cmd.Parameters.Add(recipeNameParameter);
      cmd.Parameters.Add(recipeRatingParameter);
      cmd.Parameters.Add(recipeInstructionsParameter);
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

    public static Recipe Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE id = @RecipeId ORDER BY rating DESC", conn);
      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = id.ToString();
      cmd.Parameters.Add(recipeIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundRecipeId = 0;
      string foundRecipeName = null;
      int foundRecipeRating = 0;
      string foundRecipeInstructions = null;

      while(rdr.Read())
      {
        foundRecipeId = rdr.GetInt32(0);
        foundRecipeName = rdr.GetString(1);
        foundRecipeRating = rdr.GetInt32(2);
        foundRecipeInstructions = rdr.GetString(3);
      }
      Recipe foundRecipe = new Recipe(foundRecipeName, foundRecipeRating, foundRecipeInstructions, foundRecipeId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundRecipe;
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM recipes WHERE id = @RecipeId; DELETE FROM recipes_categories WHERE recipes_id = @RecipeId;", conn);
      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();

      cmd.Parameters.Add(recipeIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddCategory(Category newCategory)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_categories (recipes_id, categories_id) VALUES (@RecipeId, @CategoryId);", conn);
      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = newCategory.GetId();
      cmd.Parameters.Add(categoryIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Category> GetCategories()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT categories.* FROM recipes JOIN recipes_categories ON (recipes.id = recipes_categories.recipes_id) JOIN categories ON (recipes_categories.categories_id = categories.id) WHERE recipes.id = @RecipeId;", conn);
      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();

      cmd.Parameters.Add(recipeIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Category> categories = new List<Category> {};

      while(rdr.Read())
        {
          int thisCategoryId = rdr.GetInt32(0);
          string categoryName = rdr.GetString(1);
          Category foundCategory = new Category(categoryName, thisCategoryId);
          categories.Add(foundCategory);
        }
        if (rdr != null)
        {
          rdr.Close();
        }

      if (conn != null)
      {
        conn.Close();
      }
      return categories;
    }

    public void AddIngredient(Ingredient newIngredient)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_ingredients (recipes_id, ingredients_id) VALUES (@RecipeId, @IngredientId);", conn);
      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      SqlParameter ingredientIdParameter = new SqlParameter();
      ingredientIdParameter.ParameterName = "@IngredientId";
      ingredientIdParameter.Value = newIngredient.GetId();
      cmd.Parameters.Add(ingredientIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Ingredient> GetIngredients()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT ingredients.* FROM recipes JOIN recipes_ingredients ON (recipes.id = recipes_ingredients.recipes_id) JOIN ingredients ON (recipes_ingredients.ingredients_id = ingredients.id) WHERE recipes.id = @RecipeId;", conn);
      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = this.GetId();

      cmd.Parameters.Add(recipeIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Ingredient> ingredients = new List<Ingredient> {};

      while(rdr.Read())
        {
          int thisIngredientId = rdr.GetInt32(0);
          string ingredientName = rdr.GetString(1);
          Ingredient foundIngredient = new Ingredient(ingredientName, thisIngredientId);
          ingredients.Add(foundIngredient);
        }
        if (rdr != null)
        {
          rdr.Close();
        }

      if (conn != null)
      {
        conn.Close();
      }
      return ingredients;
    }

    public void UpdateInstructions(string newInstructions)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand ("UPDATE recipes SET instructions = @NewInstructions OUTPUT INSERTED.instructions WHERE id = @RecipeId;", conn);

      cmd.Parameters.AddWithValue("@NewInstructions", newInstructions);
      cmd.Parameters.AddWithValue("@RecipeId", _id);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._instructions = rdr.GetString(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void UpdateRating(int newRating)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand ("UPDATE recipes SET rating = @NewRating OUTPUT INSERTED.rating WHERE id = @RecipeId;", conn);

      cmd.Parameters.AddWithValue("@NewRating", newRating);
      cmd.Parameters.AddWithValue("@RecipeId", _id);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._rating = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Recipe> SearchRecipeName(string searchRecipeName)
    {
      List<Recipe> allRecipes = new List<Recipe>{};

      SqlConnection conn = DB.Connection();
      conn.Open();


      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE recipe_name Like @RecipeName;", conn);

      SqlParameter recipeNameParam = new SqlParameter();
      recipeNameParam.ParameterName = "@RecipeName";
      recipeNameParam.Value = "%" + searchRecipeName + "%";

      cmd.Parameters.Add(recipeNameParam);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        int recipeRating = rdr.GetInt32(2);
        string recipeInstructions = rdr.GetString(3);

        Recipe newRecipe = new Recipe(recipeName, recipeRating, recipeInstructions, recipeId);
        allRecipes.Add(newRecipe);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allRecipes;
    }



    public static List<Recipe> FindRecipeByRating()
    {
      List<Recipe> allRecipes = new List<Recipe>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM recipes WHERE rating>=5 ORDER BY rating DESC;", conn);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        int recipeRating = rdr.GetInt32(2);
        string recipeInstructions = rdr.GetString(3);

        Recipe newRecipe = new Recipe(recipeName, recipeRating, recipeInstructions, recipeId);
        allRecipes.Add(newRecipe);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allRecipes;
    }
  }
}
