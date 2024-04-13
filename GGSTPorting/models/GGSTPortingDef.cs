using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonisUI;
using Newtonsoft.Json;

namespace GGSTPorting.NameChange;

public class GGSTPortingDefault
{

    [JsonProperty("character")]
    public List<Character> Character;
}

public class Character
{
    [JsonProperty("CharacterID")]
    public string ID;

    [JsonProperty("CharacterName")]
    public string Name;
}
