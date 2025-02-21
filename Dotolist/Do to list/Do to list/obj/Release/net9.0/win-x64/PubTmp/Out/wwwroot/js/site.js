document.addEventListener("DOMContentLoaded", function () {
    var canvas = document.getElementById("studyChart");
    canvas.width = 150;
    canvas.height = 150;

    var ctx = canvas.getContext("2d");

    var totalTime = studyTime + breakTime + otherTime;
    var studyPercent = totalTime > 0 ? (studyTime / totalTime * 100).toFixed(2) : 0;
    var breakPercent = totalTime > 0 ? (breakTime / totalTime * 100).toFixed(2) : 0;
    var otherPercent = totalTime > 0 ? (otherTime / totalTime * 100).toFixed(2) : 0;

    new Chart(ctx, {
        type: "doughnut",
        data: {
            labels: [
                `Study ${studyPercent}%`,
                `Break ${breakPercent}%`,
                `Other ${otherPercent}%`
            ],
            datasets: [
                {
                    data: [studyTime, breakTime, otherTime],
                    backgroundColor: ["#FF7F0E", "#1F77B4", "#A0A0A0"],
                },
            ],
        },
        options: {
            responsive: false,
            maintainAspectRatio: false,
            plugins: {
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            let value = tooltipItem.raw;
                            let percentage = totalTime > 0 ? (value / totalTime * 100).toFixed(2) : 0;
                            return `${tooltipItem.label}: ${percentage}% (${value} mins)`;
                        }
                    }
                }
            }
        },
    });

    console.log(`Study: ${studyPercent}%, Break: ${breakPercent}%, Other: ${otherPercent}%`);
});
