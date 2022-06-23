using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHCharacterData : EHTableRow
{
    public string CharacterName;
    public EHCharacter CharacterPrefab;
}

public class EHCharacterTable : EHDataTable<EHCharacterData>
{
    
}
