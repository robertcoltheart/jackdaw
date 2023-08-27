namespace Jackdaw.Configuration.Evaluation;

internal interface IConfigEvaluator
{
    void Evaluate(Dictionary<string, string> properties, GlobalConfig config);
}
