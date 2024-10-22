import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import RoleCountUserStatistics from '@services/statistics/statistics.interface';
import { StatisticsService } from '@services/statistics/statistics.service';
import { ChartData, ChartOptions, ChartType } from 'chart.js';
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
  @ViewChild(BaseChartDirective) barChart!: BaseChartDirective;
  @ViewChild(BaseChartDirective) pieChart!: BaseChartDirective;
  public isChartVisible = false;
  listRoleCountUser: RoleCountUserStatistics[] = [];

  constructor(private statisticsService: StatisticsService, private messageService: MessageService,) {
    setTimeout(() => {
      this.isChartVisible = true;
    }, 1000);
  }

  ngOnInit(): void {
    this.statisticLogMethod();
    this.statisticRoleCountUser();
  }

  public barChartLabels: string[] = [];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      title: {
        display: true,
        text: 'Biểu đồ Log Method'
      },
      legend: {
        position: 'top'
      }
    }
  };
  public barChartData: ChartData<'bar'> = {
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


  statisticLogMethod(): void {
    const startDate = new Date('2024-01-01');
    const endDate = new Date('2024-12-12');
    this.statisticsService.statisticLogMethod(startDate, endDate).subscribe({
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
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
    });
  }

  statisticRoleCountUser(): void {
    this.statisticsService.statisticRoleCountUser().subscribe({
      next: (response) => {
        this.listRoleCountUser = response.data;
        this.pieChartData.labels = this.listRoleCountUser.map(item => item.roleName);
        this.pieChartData.datasets[0].data = this.listRoleCountUser.map(item => item.totalUsers);
        this.pieChart?.chart?.update();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
    });
  }

  public pieChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
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

  public pieChartData: ChartData<'pie'> = {
    labels: this.listRoleCountUser.map(item => item.roleName),
    datasets: [{
      data: this.listRoleCountUser.map(item => item.totalUsers),
    }]
  };

}
