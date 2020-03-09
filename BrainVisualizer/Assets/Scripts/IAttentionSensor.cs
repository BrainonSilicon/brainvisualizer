// Author : Tal Ein-Gar


interface IAttentionSensor
{


    double getAttention();
    double getLikelihood();
    void getAttentionAndLikelihood(ref double attention, ref double likelihood);

    bool isActive();
}
