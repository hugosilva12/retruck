import React from 'react'
import MenuSuperAdmin from "../../components/menus/menu_manager"
import Toolbar from "../../components/utils/toolbar_manager"
import Footer from '../../components/utils/footer'
import Chart from '../../components/charts/chart'
import ChartPie from '../../components/charts/chart_pie'
import api from '../../service/api'
import { formatValueWithRegex } from '../../global/utils'

class HomeManager extends React.Component {


  constructor(props) {
    super(props)
    this.state = {
      view: 0,
      valueTotalSpend: 0,
      profitWithServices: 0,
      graph_pie: {},
      summaryTransport: {},
      summaryTransportCategories: {},
      token: "",
    }
    this.state.token = localStorage.getItem("token");
    if (this.state.token == "") {
      window.location.href = '/';
    }
    document.title = "Home"
  }


  componentWillMount() {
    const response = api.get("/api/v1/truckBreakDown/total", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response.then(result => this.setState({ valueTotalSpend: result.data }));

    const response_spend = api.get("/api/v1/service/total", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response_spend.then(result => this.setState({ profitWithServices: result.data }));


    const response_graph = api.get("/api/v1/truck/categories", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response_graph.then(result => this.setState({ graph_pie: result.data }));



    this.getChartData();



  }
  componentDidMount() {
    const response_graph = api.get("/api/v1/truck/categories", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response_graph.then(result => this.setState({ graph_pie: result.data }));

    const response2 = api.get("/api/v1/transport/statistics", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response2.then(result => this.setState({ summaryTransport: result.data }));

    const responseSummaryTransportCategories = api.get("/api/v1/transport/transportsbycategory", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    responseSummaryTransportCategories.then(result => this.setState({ summaryTransportCategories: result.data }));


  }

  //Functions to Calcule
  calculateMargin() {
    var margin = ((parseFloat(this.state.profitWithServices) / this.state.valueTotalSpend) - 1) * 100;
    margin = parseFloat(margin).toFixed(2);
    return margin;
  }

  calculateTotalTruck() {
    var truck = this.state.graph_pie.refrigerator + this.state.graph_pie.just_tractor + this.state.graph_pie.concrete_truck + this.state.graph_pie.dump_truck;
    return truck;
  }


  getChartData() {
    // Ajax calls here
    this.setState({
      chartData: {
        labels: ['GALP', 'CEPSA', 'BP'],
        datasets: [
          {
            label: 'Preço Combustível',
            data: [
              1.98,
              2.109,
              2.004,

            ],
            backgroundColor: [
              'rgb(0, 156, 255)',
              'rgb(0, 225, 250)',
              'rgb(33, 188, 205)',

            ]
          }
        ]
      },

    });

  }

  render() {
    const chartData2 = {
      labels: ['Refrigerator', 'Container', 'Cistern', 'Dump'],
      datasets: [
        {
          label: 'Preço Combustível',
          data: [
            this.state.graph_pie.refrigerator,
            this.state.graph_pie.just_tractor,
            this.state.graph_pie.concrete_truck,
            this.state.graph_pie.dump_truck,
          ],
          backgroundColor: [
            'rgb(0, 156, 255)',
            'rgb(255, 165, 0)',
            'rgb(33, 188, 205)',
            'rgb(0, 255, 0)',


          ]
        }
      ]
    }


    const summaryTransportCategoriesData = {
      labels: ['Refrigerator', 'Container', 'Cistern', 'Dump'],
      datasets: [
        {
          label: 'Transporte Por Categoria',
          data: [
            this.state.summaryTransportCategories.refrigerator,
            this.state.summaryTransportCategories.just_tractor,
            this.state.summaryTransportCategories.concrete_truck,
            this.state.summaryTransportCategories.dump_truck,
          ],
          backgroundColor: [
            'rgb(0, 156, 255)',
            'rgb(255, 165, 0)',
            'rgb(33, 188, 205)',
            'rgb(0, 255, 0)',


          ]
        }
      ]
    }


    const pieDataService = {
      labels: ['Aceites', 'Recusados'],
      datasets: [
        {
          label: 'Resultado da análise',
          data: [
            this.state.summaryTransport.accepted,
            this.state.summaryTransport.denied,

          ],
          backgroundColor: [
            'rgb(0,255,0)',
            'rgb(255, 0, 0)',
          ]
        }
      ]
    }
    return (
      <div className="container-xxl position-relative bg-white d-flex p-0">
        <MenuSuperAdmin />
        {/* Content Start */}
        <div className="content">
          <Toolbar />
          <div className="container-fluid pt-4 px-4">
            <div className="row g-4">
              <div className="col-sm-12 col-xl-12">
                <img class="img-responsive" src="img/truck3.png" alt="" style={{ width: '60%', height: '300px', display: "auto", marginLeft: "auto", marginRight: "auto" }} />
              </div>
            </div>
          </div>
          {/* Sales Chart End */}
          <div className="container-fluid pt-4 px-4">
            <div className="row g-4">
              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-arrow-down fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-0" style={{ textAlign: " center", fontSize: "18px" }}>Custos Extra</p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{formatValueWithRegex(this.state.valueTotalSpend)}€</h6>
                  </div>
                </div>
              </div>

              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-chart-bar fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-0" style={{ textAlign: " center", fontSize: "18px" }}>Ganhos</p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{formatValueWithRegex(this.state.profitWithServices)}€</h6>
                  </div>
                </div>
              </div>
              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-percent fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-0" style={{ textAlign: " center", fontSize: "18px" }}>Margem Bruta</p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{this.calculateMargin()} %</h6>
                  </div>
                </div>
              </div>
              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-truck fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-0" style={{ textAlign: " center", fontSize: "18px" }}>Camiões</p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{this.calculateTotalTruck()}</h6>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Graphs*/}
          <div className="container-fluid pt-4 px-4">
            <div class="row g-4">
              <div class="col-sm-12 col-xl-6">
                <Chart chartData={this.state.chartData} legendPosition="bottom" />
              </div>
              <div class="col-sm-12 col-xl-6">
                {this.state.graph_pie.refrigerator != undefined &&
                  <>
                    <ChartPie chartData={chartData2} legendPosition="bottom" />
                  </>
                }

              </div>
            </div>
          </div>
          <div className="container-fluid pt-4 px-4">
            <div class="row g-4">
              <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center', marginTop: 25 }}>
                <h1>Sumário de transportes
                </h1>
              </div>
              <div class="col-sm-12 col-xl-6">

                {this.state.summaryTransport.accepted != undefined &&
                  <>
                    <Chart chartData={pieDataService} legendPosition="bottom" />
                  </>
                }

              </div>
              <div class="col-sm-12 col-xl-6">
                {this.state.summaryTransportCategories.refrigerator != undefined &&
                  <>
                    <Chart chartData={summaryTransportCategoriesData} legendPosition="bottom" />
                  </>
                }

              </div>

            </div>
          </div>
          {/* Footer Start */}
          <Footer />
          {/* Footer End */}
        </div>
      </div>
    )
  }
}


export default HomeManager

