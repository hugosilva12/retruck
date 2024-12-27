using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.DTOS;
using WebApplication1.DTOS.Read;
using WebApplication1.Models;
using WebApplication1.Repository.Implementations;

namespace WebApplication1.Controllers;

/// <summary>
/// This controller manage absences.
/// </summary>
[Route("api/v1/absence")]
[ApiController]
public class AbsenceController : Controller
{
    private IFirebaseClient client;

    private readonly IAbsenceRepository absenceRepository;


    /// <summary>
    /// This constructor inject the Absence repository to be use by the Absence controller.
    /// </summary>
    /// <param name="absence"> absence repository </param>
    /// <param name="mapper"></param>
    public AbsenceController(IAbsenceRepository absence)
    {
        absenceRepository = absence;
    }

    /// <summary>
    /// This endpoint adds a new absence in sql database and firebase.
    /// </summary>
    /// <param name="absenceWriteDto">to be added</param>
    /// <returns>The added <see cref="Absence"/>, exception if the data matches an already existing absence</returns>
    [HttpPost]
    public async Task<ActionResult<Absence>> addAbsence([FromBody] AbsenceWriteDto absenceWriteDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (absenceWriteDto == null)
            return BadRequest();

        if (absenceWriteDto.description == null || absenceWriteDto.description == "")
        {
            absenceWriteDto.description = "Sem Descrição";
        }

        var absenceAdded = await absenceRepository.addAbsenceFromFirebase(absenceWriteDto);
        Console.WriteLine("ABSENCE " + absenceAdded);

        return absenceAdded;
    }

    /// <summary>
    /// This endpoint updates the specific absence of the given id.
    /// </summary>
    /// <param name="status">Absence <see cref="AbsenceUpdateWriteDto"/> with status to update absence</param>
    /// <param name="id"> Absence <see cref="Guid"/>to edit</param>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Absence>> updateAbsence(Guid id, [FromBody] AbsenceUpdateWriteDto status)
    {
        var absenceUpdated = absenceRepository.updateAbsence(id, status.status);

        // var absenceReadDto = mapper.Map<AbsenceReadDto>(absenceUpdated);

        return absenceUpdated.Result;
    }

    /// <summary>
    /// This endpoint gets all absences from firebase database and insert them into the sql database.
    /// </summary>
    [Route("/absencefirebase")]
    [HttpGet]
    public async Task<ActionResult> getAllAbsencesFromFirebase()
    {
        client = new FirebaseClient(Utils.config);
        FirebaseResponse response = client.Get("absences");
        dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
        if (data != null)
        {
            foreach (var item in data)
            {
                var register = JsonConvert.DeserializeObject<AbsenceWriteDto>(((JProperty)item).Value.ToString());
                if (register.description == null)
                {
                    register.description = "Sem Descrição";
                }

                await absenceRepository.addAbsenceFromFirebase(register);
            }
        }

        return Ok();
    }

    /// <summary>
    /// This endpoint returns all absences.
    /// </summary>
    /// <returns> List with all <see cref="AbsenceReadDto"/></returns>
    [HttpGet]
    public async Task<ActionResult<List<AbsenceReadDto>>> getAllAbsences()
    {
        var list = await absenceRepository.getAllAbsences();
        var listToReturn = new List<AbsenceReadDto>();
        foreach (var item in list)
        {
            AbsenceReadDto absenceReadDto = new AbsenceReadDto();
            absenceReadDto.abscence = item.absence;
            absenceReadDto.status = item.status;
            absenceReadDto.date = item.date.ToString("dd/MM/yyyy");
            absenceReadDto.description = item.description;
            absenceReadDto.driver = new UserReadDto();
            absenceReadDto.driver.name = item.driver.name;
            absenceReadDto.id = item.id;
            listToReturn.Add(absenceReadDto);
        }

        return listToReturn;
    }
}