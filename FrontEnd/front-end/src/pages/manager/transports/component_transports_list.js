import React from 'react';
import MenuSuperAdmin from "../../../components/menus/menu_manager"
import Toolbar from "../../../components/utils/toolbar_manager"
import api from '../../../service/api'
import { formatValueWithRegex } from '../../../global/utils'

class TransportList extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      value: 0,
      token: "",
      transports: []
    }

    this.state.token = localStorage.getItem("token");

    if (this.state.token == "") {
      window.location.href = '/';
    }

    document.title = "Ausências"

  }
  //Reload Method
  reload = async e => {
    const response2 = api.get('/transportsfirebase', {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response2.then(result => window.location.reload(false));

  }
  componentDidMount() {

    const response = api.get("/api/v1/transport", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });
    response.then(result => this.setState({ transports: result.data }));
  }

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
              <h1 >Transportes </h1>
            </div>
            <div className="col-12">
              <div className="bg-light rounded h-100 p-4">
                <div className="table-responsive">
                  <table className="table">
                    <thead>
                      <tr>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Data </th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Cliente</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Valor</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Origem</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Destino</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Tipo</th>
                        <th scope="col" style={{ textAlign: 'center', fontSize: 20 }}>Estado</th>
                      </tr>
                    </thead>
                    <tbody>
                      {this.state.transports.map((transport) => (
                        <>
                          <tr>
                            <td style={{ textAlign: 'center' }}>{transport.date}</td>
                            {transport.client_id != undefined &&
                              <>
                                <td style={{ textAlign: 'center' }}>{transport.client_id.name}</td>
                              </>
                            }
                            <td style={{ textAlign: 'center' }}>{formatValueWithRegex(transport.value_offered)} €</td>
                            <td style={{ textAlign: 'center' }}>{transport.origin}</td>
                            <td style={{ textAlign: 'center' }}>{transport.destiny}</td>
                            <td style={{ textAlign: 'center' }}>{transport.truckCategory}</td>

                            {transport.status == 'WAIT_APROVE' &&
                              <>
                                <td style={{ textAlign: 'center' }}>   <button type="button" class="btn btn-warning rounded-pill m-2">Pendente</button></td>
                              </>
                            }
                            {transport.status == 'ACCEPT' &&
                              <>
                                <td style={{ textAlign: 'center' }}>   <button type="button" class="btn btn-success rounded-pill m-2">Aceitado</button></td>
                              </>
                            }

                            {transport.status == 'REJECTED' &&
                              <>
                                <td style={{ textAlign: 'center' }}><button type="button" class="btn btn-danger rounded-pill m-2">Recusado</button></td>
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
      </div>
    )
  }
}


export default TransportList

