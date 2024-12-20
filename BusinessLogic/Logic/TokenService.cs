﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogic.Logic;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly IConfiguration _config;

    public TokenService( IConfiguration config)
    {    _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:key"]));
       
    }

    public string CreateToken(Usuario usuario)
    {
        var claims = new List<Claim> {
         new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
         new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre),
         new Claim(JwtRegisteredClaimNames.FamilyName, usuario.Apellido),
         new Claim("username", usuario.UserName),

        };

        // desencriptar token
        var credentials = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

        var tokenConfig = new SecurityTokenDescriptor { 
         Subject = new ClaimsIdentity(claims),
         Expires = DateTime.Now.AddDays(1),
         SigningCredentials = credentials,
         Issuer = _config["Token:Issuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);
        //retornando el token en string
        return tokenHandler.WriteToken(token);
    }
}
