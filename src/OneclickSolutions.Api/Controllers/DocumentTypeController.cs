using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneClickSolutions.Application.DocumentTypes.Commands;
using OneClickSolutions.Application.DocumentTypes.ViewModels;
using OneClickSolutions.Domain.DocumentTypes;
using OneClickSolutions.Domain.DocumentTypes.Catalogs;
using OneClickSolutions.Domain.DocumentTypes.Repositories;
using OneClickSolutions.Infrastructure.Domain;
using OneClickSolutions.Infrastructure.EntityFrameworkCore.Context;

namespace OneclickSolutions.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public DocumentTypeController( IMapper mapper, IMediator mediator)
        {

            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddDocumentTypeViewModel request)
        {

            var createDocumentTypeCommpand=_mapper.Map<AddDocumentTypeViewModel, CreateDocumentTypeCommand>(request);
            var result= await _mediator.Send(createDocumentTypeCommpand);
            return Ok(result);
        }
    }
}
