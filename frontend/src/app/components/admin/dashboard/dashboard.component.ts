import { StoreSidebar } from './../../../store/store.sidebar';
import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import RoleCountUserStatistics from '@services/statistics/statistics.interface';
import { StatisticsService } from '@services/statistics/statistics.service';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [BaseChartDirective, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  @ViewChild('barChart') barChart!: BaseChartDirective;
  @ViewChild('lineChart') lineChart!: BaseChartDirective;
  @ViewChild('pieChart') pieChart!: BaseChartDirective;
  isSidebarVisible = true;
  isChartVisible = false;
  listRoleCountUser: RoleCountUserStatistics[] = [];
  barChartLabels: string[] = [];
  lineChartLabels: string[] = [];
  month: string = '';

  constructor(
    private statisticsService: StatisticsService,
    private messageService: MessageService,
    private storeSidebar: StoreSidebar
  ) {
    setTimeout(() => {
      this.isChartVisible = true;
    }, 700);
  }

  ngOnInit(): void {
    this.getLogMethodByYear();
    this.getRoleCountUser();
    this.getLogMethodByMonth();
  }

  ngAfterViewInit(): void {
    this.storeSidebar.sidebarVisible$.subscribe((isVisible) => {
      this.isSidebarVisible = isVisible;

      // Tự động cập nhật kích thước biểu đồ khi ẩn/hiện sidebar
      if (this.barChart && this.barChart.chart) {
        this.barChart.chart.resize();
      }
      if (this.lineChart && this.lineChart.chart) {
        this.lineChart.chart.resize();
      }
      if (this.pieChart && this.pieChart.chart) {
        this.pieChart.chart.resize();
      }
    });
  }

  getLogMethodByYear(): void {
    const startDate = new Date('2024-01-01');
    const endDate = new Date('2024-12-12');
    this.statisticsService.getLogMethodByYear(startDate, endDate).subscribe({
      next: (response) => {
        this.barChartLabels = response.data.map(item => item.yearMonth);
        this.barChartData.datasets[0].data = response.data.map(item => item.postCount);
        this.barChartData.datasets[1].data = response.data.map(item => item.putCount);
        this.barChartData.datasets[2].data = response.data.map(item => item.getCount);
        this.barChartData.datasets[3].data = response.data.map(item => item.deleteCount);
        this.barChartData.labels = this.barChartLabels;
        this.barChart?.chart?.update();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  getLogMethodByMonth(): void {
    const startDate = new Date('2024-10-10');
    let cutTime: string[] = [];
    this.statisticsService.getLogMethodByMonth(startDate).subscribe({
      next: (response) => {
        cutTime = response.data.map(item => item.accessDate.split('T')[0])
        this.month = cutTime[0].split('-')[1];
        console.log(this.month);

        this.lineChartLabels = cutTime.map(item => item.split('-')[2]);
        this.lineChartData.datasets[0].data = response.data.map(item => item.postCount);
        this.lineChartData.datasets[1].data = response.data.map(item => item.putCount);
        this.lineChartData.datasets[2].data = response.data.map(item => item.getCount);
        this.lineChartData.datasets[3].data = response.data.map(item => item.deleteCount);

        this.lineChartOptions.plugins!.title!.text = `Biểu đồ LogMethod tháng ${this.month}`;
        this.lineChartData.labels = this.lineChartLabels;
        this.lineChart?.chart?.update();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  getRoleCountUser(): void {
    this.statisticsService.getRoleCountUser().subscribe({
      next: (response) => {
        this.listRoleCountUser = response.data;
        this.pieChartData.labels = this.listRoleCountUser.map(item => item.roleName);
        this.pieChartData.datasets[0].data = this.listRoleCountUser.map(item => item.totalUsers);
        this.pieChart?.chart?.update();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  barChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    animation: {
      duration: 1000, // Thời gian hoạt hình
      easing: 'easeOutBounce', // Phương thức làm mềm
      onComplete: () => {
        console.log('Animation Complete!');
      }
    },
    plugins: {
      title: {
        display: true,
        text: 'Biểu đồ LogMethod theo năm'
      },
      legend: {
        position: 'top'
      }
    }
  };

  lineChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    animation: {
      duration: 1000,
      easing: 'easeOutBounce',
      onComplete: () => {
        console.log('Animation Complete!');
      }
    },
    plugins: {
      title: {
        display: true,
        text: `Biểu đồ LogMethod tháng ${this.month}`
      },
      legend: {
        position: 'top'
      }
    }
  };

  pieChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    animation: {
      duration: 1000,
      easing: 'easeOutBounce',
      onComplete: () => {
        console.log('Animation Complete!');
      }
    },
    plugins: {
      title: {
        display: true,
        text: 'Biểu đồ Role'
      },
      legend: {
        position: 'top'
      }
    }
  };

  barChartData: ChartData<'bar'> = {
    labels: this.barChartLabels,
    datasets: [
      {
        data: [],
        label: 'POST',
        borderColor: '#2d8a4e',
        backgroundColor: 'rgba(45, 138, 78, 0.5)',
      },
      {
        data: [],
        label: 'PUT',
        borderColor: '#d68b00',
        backgroundColor: 'rgba(214, 139, 0, 0.5)',
      },
      {
        data: [],
        label: 'GET',
        borderColor: '#236c85',
        backgroundColor: 'rgba(35, 108, 133, 0.5)',
      },
      {
        data: [],
        label: 'DELETE',
        borderColor: '#a00428',
        backgroundColor: 'rgba(160, 4, 40, 0.5)',
      }
    ]
  };

  lineChartData: ChartData<'line'> = {
    labels: this.lineChartLabels,
    datasets: [
      {
        data: [],
        label: 'POST',
        borderColor: '#2d8a4e',
        backgroundColor: 'rgba(45, 138, 78, 0.5)',
      },
      {
        data: [],
        label: 'PUT',
        borderColor: '#d68b00',
        backgroundColor: 'rgba(214, 139, 0, 0.5)',
      },
      {
        data: [],
        label: 'GET',
        borderColor: '#236c85',
        backgroundColor: 'rgba(35, 108, 133, 0.5)',
      },
      {
        data: [],
        label: 'DELETE',
        borderColor: '#a00428',
        backgroundColor: 'rgba(160, 4, 40, 0.5)',
      }
    ]
  };

  pieChartData: ChartData<'pie'> = {
    labels: this.listRoleCountUser.map(item => item.roleName),
    datasets: [{
      data: this.listRoleCountUser.map(item => item.totalUsers),
    }]
  };

}
