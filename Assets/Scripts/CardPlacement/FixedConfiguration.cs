using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FixedConfiguration is an implementation of IStageConfiguration
// responsible for reproducing same card configuration for each state.
//
// Stage component will call RecordStageConfiguration() during initialization,
// which sends and stores its card configuration as FixedConfiguration instance.
public class FixedConfiguration : IStageConfiguration
{
    private List<CardType> cardConfiguration;

    public FixedConfiguration(List<CardType> cardConfiguration)
    {
        this.cardConfiguration = cardConfiguration;
    }

    public List<CardType> GetStageConfiguration(int numCards)
    {
        return cardConfiguration;
    }
}
