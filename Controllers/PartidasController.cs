using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SccpMvcApi.Models;

namespace SccpMvcApi.Controllers
{
    public class PartidasController : Controller
    {
         public string uriBase = "http://sccpapi.somee.com/Partidas/";

    [HttpGet]
    public async Task<ActionResult> IndexAsync()
    {
      try
      {
        string uriComplementar = "GetAll";
        HttpClient httpClient = new HttpClient();
        //string token = HttpContext.Session.GetString("SessionTokenUsuario");
        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          List<PartidasViewModel> listaPartidas = await Task.Run(() =>
          JsonConvert.DeserializeObject<List<PartidasViewModel>>(serialized));

          return View(listaPartidas);

        }
        else
          throw new System.Exception(serialized);

      }

      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }


    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(PartidasViewModel p)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent(JsonConvert.SerializeObject(p));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await httpClient.PostAsync(uriBase, content);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] = string.Format("Partida [0], Id {1} salva com sucesso!", p.Nome, serialized);
          return RedirectToAction("Index");
        }

        else
          throw new System.Exception(serialized);
      }

      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Create");
      }
    }

    [HttpGet]
    public ActionResult Create()
    {
      return View();
    }

    [HttpGet]
    public async Task<ActionResult> DetailsAsync(int? id)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          PartidasViewModel p = await Task.Run(() =>
              JsonConvert.DeserializeObject<PartidasViewModel>(serialized));
          return View(p);
        }
        else
          throw new System.Exception(serialized);
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View("Error!");
    }

    [HttpGet]
    public async Task<ActionResult> EditAsync(int? id)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await httpClient.GetAsync(uriBase + id.ToString());

        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          PartidasViewModel p = await Task.Run(() =>
              JsonConvert.DeserializeObject<PartidasViewModel>(serialized));
          return View(p);
        }
        else
          throw new System.Exception(serialized);
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    public async Task<ActionResult> EditAsync(PartidasViewModel p)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(JsonConvert.SerializeObject(p));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await httpClient.PutAsync(uriBase, content);
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] =
                  string.Format("Partida {0}, dia {1} atualizado com sucesso!", p.Nome, p.Dia);

          return RedirectToAction("Index");
        }
        else
          throw new System.Exception(serialized);
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }

    [HttpGet]
    public async Task<ActionResult> DeleteAsync(int id)
    {
      try
      {
        HttpClient httpClient = new HttpClient();
        string token = HttpContext.Session.GetString("SessionTokenUsuario");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.DeleteAsync(uriBase + id.ToString());
        string serialized = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
          TempData["Mensagem"] = string.Format("Partida Id {0} removida com sucesso!", id);
          return RedirectToAction("Index");
        }
        else
        {
          throw new System.Exception(serialized);
        }
      }
      catch (System.Exception ex)
      {
        TempData["MensagemErro"] = ex.Message;
        return RedirectToAction("Index");
      }
    }

    }
}