using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.ContactDtos;
using Bookory.Business.Utilities.DTOs.GenreDtos;
using Bookory.Business.Utilities.Exceptions.BlogException;
using Bookory.Business.Utilities.Exceptions.ContactExceptions;
using Bookory.Business.Utilities.Exceptions.GenreExceptions;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Implementations;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Bookory.Business.Services.Implementations;

public class ContactService : IContactService
{
    private readonly IMapper _mapper;
    private readonly IContactRepository _contactRepository;

    public ContactService(IMapper mapper, IContactRepository contactRepository)
    {
        _mapper = mapper;
        _contactRepository = contactRepository;
    }

    public async Task<List<ContactGetResponseDto>> GetAllContactMessagesAsync(string? search)
    {
        var contacts = await _contactRepository.GetFiltered(
          b => string.IsNullOrEmpty(search) || b.Name.ToLower().Contains(search.Trim().ToLower())).ToListAsync();

        if (contacts == null || contacts.Count == 0)
            throw new ContactNotFoundException("No blogs were found matching the search criteria.");

        var contactDtos = _mapper.Map<List<ContactGetResponseDto>>(contacts);
        return contactDtos;
    }

    public async Task<ContactGetResponseDto> GetContactMessageByIdAsync(Guid id)
    {
        var contact = await _contactRepository.GetSingleAsync(b => b.Id == id);

        if (contact is null)
            throw new ContactNotFoundException($"The contact message with ID {id} was not found");

        var contactDto = _mapper.Map<ContactGetResponseDto>(contact);
        return contactDto;
    }


    public async Task<ResponseDto> CreateContactMessageAsync(ContactPostDto contactPostDto)
    {
        var newContactMessage = _mapper.Map<Contact>(contactPostDto);

        await _contactRepository.CreateAsync(newContactMessage);
        await _contactRepository.SaveAsync();

        return new((int)HttpStatusCode.Created, "Contact successfully created");
    }

    public async Task<ResponseDto> DeleteContactMessagesAsync(Guid id)
    {
        var contactMessage = await _contactRepository.GetByIdAsync(id);
        if (contactMessage is null)
            throw new ContactNotFoundException($"No contact found with the ID: {id}");

        _contactRepository.SoftDelete(contactMessage);
        await _contactRepository.SaveAsync();
        return new((int)HttpStatusCode.OK, "Contact successfully deleted");
    }

   
}
