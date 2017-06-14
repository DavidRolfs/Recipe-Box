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
  }
}
