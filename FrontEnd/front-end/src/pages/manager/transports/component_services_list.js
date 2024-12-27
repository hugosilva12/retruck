import React from 'react';
import MenuSuperAdmin from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import api from '../../../service/api'


class ServicesList extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      value: 0,
      token: "",
      services: []
    }

    this.state.token = localStorage.getItem("token");

    if (this.state.token == "") {
      window.location.href = '/';
    }
    document.title = "Serviços"
  }

  serviceDetails = (idService) => {
    window.location.href = '/services/' + idService;
  }

  componentDidMount() {

    const response = api.get('/api/v1/service', {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response.then(result => this.setState({ services: result.data }));
  }


  reload = async e => {

    const response2 = api.get('/api/v1/service/updateStateService', {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    const response3 = api.get('/location_services', {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response2.then(result => window.location.reload(false));

  };


  render() {

    return (
      <div className="container-xxl position-relative bg-white d-flex p-0">
        <MenuSuperAdmin />
        {/* Content Start */}
        <div className="content">
          <Toolbar />
          {/* Form Start */}
          <div className="container-fluid pt-4 px-4">
            <div className="col-sm-12 col-xl-12" style={{ textAlign: 'center' }}>
              <h1 >Serviços</h1>
            </div>
            <div className="col-12">
              <div className="bg-light rounded h-100 p-4">
                <div className="table-responsive">
                  <table className="table">
                    <thead>
                      <tr>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>#</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Data</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Origem</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Destino</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Camião</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Status</th>
                      </tr>
                    </thead>
                    <tbody>
                      {this.state.services.map((service, index) => (
                        <>
                          <tr>
                            <th scope="row" style={{ textAlign: 'center' }}>{index}</th>

                            {service.transportReadDto != undefined &&
                              <>
                                <td style={{ textAlign: 'center' }}> {service.transportReadDto.date}</td>
                              </>
                            }
                            {service.transportReadDto != undefined &&
                              <>
                                <td style={{ textAlign: 'center' }}> {service.transportReadDto.origin}</td>
                              </>
                            }
                            {service.transportReadDto != undefined &&
                              <>
                                <td style={{ textAlign: 'center' }}> {service.transportReadDto.destiny}</td>
                              </>
                            }
                            {service.truckReadDto != undefined &&
                              <>
                                <td style={{ textAlign: 'center' }}> {service.truckReadDto.matricula}</td>
                              </>
                            }
                            {service.status == 'IN_PROGRESS' &&
                              <>
                                <td style={{ textAlign: 'center' }}>   <button type="button" onClick={() => this.serviceDetails(service.id)} class="btn btn-warning rounded-pill m-2">A decorrer</button></td>
                              </>
                            }
                            {service.status == 'FINISHED' &&
                              <>
                                <td style={{ textAlign: 'center' }}>   <button type="button" onClick={() => this.serviceDetails(service.id)} class="btn btn-success rounded-pill m-2">Concluído</button></td>
                              </>
                            }

                            {service.status == 'TO_START' &&
                              <>
                                <td style={{ textAlign: 'center' }}><button type="button" onClick={() => this.serviceDetails(service.id)} class="btn btn-danger rounded-pill m-2">Por Iniciar</button></td>
                              </>
                            }
                          </tr>
                        </>
                      ))}

                    </tbody>
                  </table>
                  <button type="button" onClick={this.reload} class="btn btn-primary rounded-pill m-2">Carregar Atualizações</button>
                </div>
              </div>
            </div>
          </div>
        </div>
        {/* Content End */}
      </div>
    )
  }
}



export default ServicesList

