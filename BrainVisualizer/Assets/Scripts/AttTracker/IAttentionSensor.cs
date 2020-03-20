// Author : Tal Ein-Gar


interface IAttentionSensor
{


    double Attention();
    double Likelihood();
    void AttentionAndLikelihood(ref double attention, ref double likelihood);

    bool IsActive();
}
