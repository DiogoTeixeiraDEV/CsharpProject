using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;


namespace Api.Controllers;

[ApiController]
[Route("carts")]
[Authorize]
[SwaggerTag("Endpoints para gerenciamento de carrinho de compras")]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Obtém o carrinho do usuário", Description = "Obtém os detalhes do carrinho de compras do usuário autenticado.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserFromIdToken();
        var cart = await _cartService.GetCartByUserIdAsync(userId);
        return Ok(cart);

    }

    [HttpPost("items")]
    [SwaggerOperation(Summary = "Adiciona um item ao carrinho", Description = "Adiciona um item ao carrinho de compras do usuário autenticado.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddItemTocart([FromBody] CartItemDto cartItemDto)
    {
        var userId = GetUserFromIdToken();
        var updatedCart = await _cartService.AddItemAsync(userId, cartItemDto.ProductId, cartItemDto.Quantity);
        return Ok(updatedCart);
    }

    [HttpDelete("items/{productId}")]
    [SwaggerOperation(Summary = "Remove um item do carrinho", Description = "Remove um item do carrinho de compras do usuário autenticado.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveItemFromCart([FromRoute] Guid productId)
    {
        var userId = GetUserFromIdToken();
        var updatedCart = await _cartService.RemoveItemAsync(userId, productId);
        return Ok(updatedCart);
    }


    private Guid GetUserFromIdToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID não encontrado no Token.");
        }
        return Guid.Parse(userIdClaim.Value);
    }
}