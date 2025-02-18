using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Desafio_API_Atualizado.Context;
using Desafio_API_Atualizado.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Desafio_API_Atualizado.Endpoints
{
    public static class TarefaEndpoints
    {
        public static void MapTarefaEndpoints(this WebApplication app)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
            app.MapGet("/tarefa/{id}", (int id, OrganizadorContext context) => {
                var tarefa = context.Tarefas.Find(id);

                if(tarefa == null)
                    return Results.NotFound("Tarefa não encontrada.");
                else
                    return Results.Ok(tarefa);
            });

            // TODO: Buscar todas as tarefas no banco utilizando o EF
            app.MapGet("/tarefa", (OrganizadorContext context) => Results.Ok(context.Tarefas.ToList()));

            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            app.MapGet("/tarefa/titulo/{titulo}",(string titulo, OrganizadorContext context) =>{
                var tarefas = context.Tarefas.Where(x => x.Titulo == titulo).ToList();

                if(tarefas.Any())
                    return Results.Ok(tarefas);
                else
                    return Results.NotFound($"Não encontrada tarefas com o título {titulo}.");
            });

            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            app.MapGet("/tarefa/status/{codigo}", (int codigo, OrganizadorContext context) =>{
                if(Enum.IsDefined(typeof(EnumStatusTarefa), codigo))
                {
                    var status = (EnumStatusTarefa)Enum.ToObject(typeof(EnumStatusTarefa), codigo);
                    var tarefas = context.Tarefas.Where(x => x.Status == status).ToList();
                    if(tarefas.Any())
                        return Results.Ok(tarefas);
                    else
                        return Results.NotFound($"Não encontrada tarefas com o status {codigo}.");
                } 
                else
                    return Results.BadRequest($"Status {codigo} não existe.");

                
            });

            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            app.MapPost("/tarefa", (Tarefa tarefa, OrganizadorContext context) =>{
                context.Tarefas.Add(tarefa);
                context.SaveChanges();
                return Results.Created($"/tarefa/{tarefa.Id}", tarefa);
            });

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            app.MapPut("/tarefa/{id}", (int id, Tarefa tarefa, OrganizadorContext context) =>{
                var tarefaBd = context.Tarefas.Find(id);
                
                if(tarefaBd == null)
                    return Results.NotFound("Tarefa não encontrada.");
                else
                {
                    tarefaBd.Data = tarefa.Data;
                    tarefaBd.Descricao = tarefa.Descricao;
                    tarefaBd.Status = tarefa.Status;
                    tarefaBd.Titulo = tarefa.Titulo;
                    context.SaveChanges();

                    return Results.Ok(tarefaBd);
                }
                
            });

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            app.MapDelete("/tarefa/{id}", (int id, OrganizadorContext context) =>{
                var tarefaBd = context.Tarefas.Find(id);

                 if(tarefaBd == null)
                    return Results.NotFound("Tarefa não encontrada.");
                else
                {
                    context.Tarefas.Remove(tarefaBd);
                    context.SaveChanges();
                    return Results.NoContent();
                }
                
            });
        }
    }
}