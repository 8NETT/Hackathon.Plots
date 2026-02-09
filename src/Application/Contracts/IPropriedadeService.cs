using Application.DTOs;
using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts;

public interface IPropriedadeService
{
    Task<Result<PropriedadeDTO>> ObterAsync(Guid id);
    Task<IEnumerable<PropriedadeDTO>> ObterDoProprietarioAsync(Guid id);
    Task<Result<PropriedadeDTO>> CadastrarAsync(CadastrarPropriedadeDTO dto);
}
