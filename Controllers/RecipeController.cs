﻿using Microsoft.AspNetCore.Mvc;
using NamNamAPI.Business;
using NamNamAPI.Domain;
using NamNamAPI.Utility;
using System.Collections.Generic;

namespace NamNamAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private RecipeProvider recipeProvider;
        private InstructionProvider instructionProvider;

        private IngredientProvider ingredientProvider;

        public RecipeController([FromBody] RecipeProvider _recipeProvider, [FromBody] InstructionProvider _instructionProvider, [FromBody] IngredientProvider _ingredientProvider)
        {
            recipeProvider = _recipeProvider;
            instructionProvider = _instructionProvider;
            ingredientProvider = _ingredientProvider;

        }

        [HttpGet("GetCookbook/{idUser}")]
        public ActionResult GetCookbook(string idUser)
        {
            try
            {
                List<RecipeDomain> recipeList = recipeProvider.GetCookbook(idUser);

                if (recipeList == null || recipeList.Count == 0)
                {
                    return NotFound("No se encontraron recetas para este usuario.");
                }
                return Ok(recipeList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener recetas: {ex.Message}");

                return StatusCode(500, "Se produjo un error al procesar la solicitud.");
            }

        }

        [HttpPost("PostRecipe")]
        public ActionResult PostRecipe([FromBody] NewRecipeDomain newRecipeDomain)
        {
            int codeInstruction = 0;
            //primero guarda la foto y regreso el id para guardarla en recipe

            //REGISTRO DE RECIPE Y RECIPE_HAS_INGREDIENTS
            (int codeRecipe, string idRecipe, string error) = recipeProvider.PostRecipe(newRecipeDomain.recipeDomain, newRecipeDomain.category);
            if (codeRecipe == 2)
            {
                //REGISTRO DE INSTRUCTIONS
                codeInstruction = instructionProvider.PostInstruction(newRecipeDomain.instructions, idRecipe);
                if (codeInstruction == newRecipeDomain.instructions.Count)
                {
                    //REGISTRO DE INGREDIENTS
                    int codeIngredients = ingredientProvider.setRecipeHasIngredients(newRecipeDomain.recipeHasIngredients, idRecipe);
                    if (codeIngredients == 200)
                        return Ok();
                }
            }
            return StatusCode(500, error);
        }

        [HttpGet("GetRecipe/{idRecipe}")]
        public ActionResult GetRecipe(string idRecipe)
        {
            int code = 0;
            GetRecipeResponse recipe = new GetRecipeResponse();
            try
            {
                (code, recipe) = recipeProvider.getRecipe(idRecipe);
                Console.WriteLine("CODIGO--------------"+ code);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
            if(code == 200)
            return Ok(recipe);
            else
            return StatusCode(404);
        }

    }

}
