//Bang thong ke
const labels = ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']

const data = {
    labels: labels,
    datasets: [
        {
            label: 'Lượt truy cập',
            borderColor: 'rgb(75, 192, 142)',
            backgroundColor: 'rgba(75, 192, 192,0.2)',
            data: [0, 12, 58, 4, 60, 40, 30, 89, 100, 10, 15, 17],
            tension: 0.4,
            fill: 1,
        },
        {
            label: 'Người đăng ký',
            borderColor: 'red',
            backgroundColor: 'red',
            data: [8, 36, 40, 60, 30, 17, 55, 80, 21, 19, 8, 35],
            tension: 0.4,
        },
    ]
}
const config = {
    type: 'line',
    data: data,
}
const canvas = document.getElementById('canvas-btk')
const chart = new Chart(canvas, config)

/*pie chart*/
const datapie = {
    labels: [
        'Other',
        'Song 1',
        'Song 2',
        'Song 3',
    ],
    datasets: [{
        label: 'Top 3 song',
        data: [7000, 5000, 2500, 1000],
        backgroundColor: ['rgb(255, 99, 132)', 'rgb(54, 162, 235)', 'rgb(255, 205, 86)', 'rgb(255, 205, 255)'],
        hoverOffset: 10,
    }]
}
const configpie = {
    type: 'pie',
    data: datapie,
    options: {
        maintainAspectRatio: false,
        tooltips: {
            backgroundColor: "rgb(255,255,255)",
            bodyFontColor: "#858796",
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            caretPadding: 10,
        },
        plugins: {
            legend: {
                display: false
            },
        },
        cutoutPercentage: 80,
    },
}
const canvaspie = document.getElementById('canvas-pie')
const chartpie = new Chart(canvaspie, configpie)