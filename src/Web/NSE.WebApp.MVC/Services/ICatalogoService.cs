﻿using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogoService
    {

        Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null);
        Task<ProdutoViewModel> ObterPorId(Guid id);

    }
}
