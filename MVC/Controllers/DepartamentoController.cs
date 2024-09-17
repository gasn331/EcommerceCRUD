using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using MVC.Services.Interfaces;
using Shared.DTOs;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class DepartamentoController : BaseController
    {
        private IDepartamentoService _departamentoService;

        public DepartamentoController(IDepartamentoService departmentoService)
        {
            _departamentoService = departmentoService;
        }

        public async Task<IActionResult> Index()
        {
            var departamentos = await _departamentoService.GetDepartamentosAsync();
            return View(departamentos);
        }

        public async Task<IActionResult> Details(string codigo)
        {
            var departamento = await _departamentoService.GetDepartamentoAsync(codigo);

            if(departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }
    }
}
