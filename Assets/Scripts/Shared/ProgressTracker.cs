using System;
using UnityEngine;

public class ProgressTracker : MonoBehaviour {

    public event EventHandler<ValueChangedEvent<double>> OnProgressChanged;
    public event EventHandler OnProgressComplete;

    [SerializeField] private bool resetOnComplete = false;

    public double WorkDone => progress * totalWork;

    private double totalWork = 1d;
    public double TotalWork => totalWork;

    [SerializeField, Range(0f, 1f)] private double progress = 0d;
    public double Progress => progress;
    
    public bool IsComplete => progress >= 1f;
    public bool AnyProgress => progress > 0f;
    public bool HasNoProgress => progress == 0f;

    public void SetProgress(double progress) {
        // Clamp the progress between 0 and 1.
        progress = Math.Max(0d, Math.Min(1d, progress));

        var oldProgress = this.Progress;
        if (oldProgress == progress) // Don't do anything if the progress hasn't changed.
            return;

        this.progress = progress;

        OnProgressChanged?.Invoke(this, new ValueChangedEvent<double>(oldProgress, progress));

        if (progress >= 1f) {
            OnProgressComplete?.Invoke(this, EventArgs.Empty);
            if (resetOnComplete)
                ResetProgress();
        }
    }

    public void SetTotalWork(double totalWork, double workDone = 0d) {
        this.totalWork = totalWork;
        SetWorkDone(workDone);
    }

    public void SetWorkDone(double workDone) {
        SetProgress(workDone / totalWork);
    }

    public void AddWorkDone(double workDone) {
        SetWorkDone(WorkDone + workDone);
    }

    public void RemoveWorkDone(double workDone) {
        SetWorkDone(WorkDone - workDone);
    }

    public void SetWorkRemaining(double workRemaining) {
        SetProgress(1f - (workRemaining / totalWork));
    }

    public void ResetTotalWork() => SetTotalWork(1d);
    public void ResetProgress() => SetProgress(0d);

    public double GetWorkRemaining() => totalWork - WorkDone;


#if UNITY_EDITOR
    private void OnValidate() {
        if (Application.isPlaying)
            return;

        SetProgress(progress);
    }
#endif
    
}
