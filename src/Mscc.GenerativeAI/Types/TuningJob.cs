using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class ListTuningJobResponse
    {
        public List<TuningJob> TuningJobs { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{Name} ({BaseModel}): {Endpoint}")]
    public class TuningJob
    {
        /// <summary>
        /// Name of the tuned model.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Display name of the tuned model.
        /// </summary>
        public string TunedModelDisplayName { get; set; } = string.Empty;
        /// <summary>
        /// Name of the foundation model to tune.
        /// </summary>
        /// <remarks>
        /// Supported values: gemini-1.5-pro-002, gemini-1.5-flash-002, and gemini-1.0-pro-002.
        /// </remarks>
        public string BaseModel { get; set; } = string.Empty;
        /// <summary>
        /// Output only. Time when the Job was created.
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TunedModel? TunedModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Experiment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TuningDataStats? TuningDataStats { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public StateTuningJob State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SupervisedTuningSpec? SupervisedTuningSpec { get; set; }
        /// <summary>
        /// Output only. Only populated when the job’s state is JOB_STATE_FAILED or JOB_STATE_CANCELLED.
        /// </summary>
        public Status? Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Endpoint => TunedModel?.Endpoint;
        /// <summary>
        /// 
        /// </summary>
        public bool HasEnded => EndTime.HasValue;
    }
}

/*
{
  "name": "projects/646763706049/locations/us-central1/tuningJobs/6284765961371254784",
  "tunedModelDisplayName": "gemini-1.5-pro-002-bf6ead6f-99a5-4ba7-8919-c84a2c30ebbb",
  "baseModel": "gemini-1.5-pro-002",
  "supervisedTuningSpec": {
    "trainingDatasetUri": "gs://cloud-samples-data/ai-platform/generative_ai/gemini-1_5/text/sft_train_data.jsonl",
    "hyperParameters": {
      "epochCount": "10",
      "learningRateMultiplier": 1,
      "adapterSize": "ADAPTER_SIZE_FOUR"
    }
  },
  "state": "JOB_STATE_FAILED",
  "createTime": "2024-11-03T21:08:07.734198Z",
  "startTime": "2024-11-03T21:08:30.888568Z",
  "endTime": "2024-11-03T21:40:37.139132Z",
  "updateTime": "2024-11-03T21:40:37.139132Z",
  "error": {
    "code": 13,
    "message": "Failed to create endpoint. Tuned model is uploaded and can be accessed through Model Registry."
  },
  "experiment": "projects/646763706049/locations/us-central1/metadataStores/default/contexts/tuning-experiment-20241103131117085279",
  "tunedModel": {
    "model": "projects/646763706049/locations/us-central1/models/1402067540926005248@1"
  },
  "tuningDataStats": {
    "supervisedTuningDataStats": {
      "tuningDatasetExampleCount": "500",
      "userInputTokenDistribution": {
        "sum": "233592",
        "min": 25,
        "max": 2932,
        "mean": 467.184,
        "median": 414,
        "p5": 101,
        "p95": 1016,
        "buckets": [
          {
            "count": 326,
            "left": 25,
            "right": 509
          },
          {
            "count": 148,
            "left": 510,
            "right": 994
          },
          {
            "count": 20,
            "left": 995,
            "right": 1478
          },
          {
            "count": 5,
            "left": 1479,
            "right": 1963
          },
          {
            "count": 1,
            "left": 2449,
            "right": 2932
          }
        ]
      },
      "userOutputTokenDistribution": {
        "sum": "21651",
        "min": 3,
        "max": 237,
        "mean": 43.302,
        "median": 37,
        "p5": 15,
        "p95": 89,
        "buckets": [
          {
            "count": 298,
            "left": 3,
            "right": 42
          },
          {
            "count": 164,
            "left": 43,
            "right": 81
          },
          {
            "count": 29,
            "left": 82,
            "right": 120
          },
          {
            "count": 5,
            "left": 121,
            "right": 159
          },
          {
            "count": 3,
            "left": 160,
            "right": 198
          },
          {
            "count": 1,
            "left": 199,
            "right": 237
          }
        ]
      },
      "userMessagePerExampleDistribution": {
        "sum": "1000",
        "min": 2,
        "max": 2,
        "mean": 2,
        "median": 2,
        "p5": 2,
        "p95": 2,
        "buckets": [
          {
            "count": 500,
            "left": 2,
            "right": 2
          }
        ]
      },
      "userDatasetExamples": [
        {
          "role": "user",
          "parts": [
            {
              "text": "Honesty is usually the best policy. It is disrespectful to lie to someone. If you don\u0027t want to date someone, you should say so.  Sometimes it is easy to be honest. For example, you might be able to truthfully say, \"No, thank you, I already have a date for that party.\" Other times, you might need to find a kinder way to be nice. Maybe you are not attracted to the person. Instead of bluntly sayi..."
            }
          ]
        },
        {
          "role": "model",
          "parts": [
            {
              "text": "Tell the truth. Use a \"compliment sandwich\". Be direct. Treat the person with respect. Communicate effectively."
            }
          ]
        },
        {
          "role": "user",
          "parts": [
            {
              "text": "Go online and simply search “Decor Style Quizzes” in order to get some direction. These quizzes will determine your personal style by having you choose between different rooms, accessories, and pieces of furniture. As you take the quiz, reflect on whether or not you could see yourself relaxing and/or entertaining guests in each space. Some styles that these quizzes may determine you have includ..."
            }
          ]
        },
        {
          "role": "model",
          "parts": [
            {
              "text": "Take a quiz online to find your style. Look online and in magazines for inspiration. Go through what you already own before you purchase new pieces. Match your decor to your personality and lifestyle."
            }
          ]
        },
        {
          "role": "user",
          "parts": [
            {
              "text": "The size of a shuffleboard table can range anywhere from nine feet to twenty-two feet. Keep in mind the amount of space you have to build and store it. You wouldn’t want to build a fourteen foot long table in a twelve foot shed. Remember you need at least two feet of additional space on each end of the table to actually play. The size you decide on will affect what dimensions and plans you use ..."
            }
          ]
        },
        {
          "role": "model",
          "parts": [
            {
              "text": "Determine what size you want your table to be. Plan or find dimensions for each part of the table. Decide on the type of wood for the playing surface."
            }
          ]
        }
      ],
      "totalBillableTokenCount": "255243"
    }
  }
}
*/