import React from 'react'
import MenuSuperAdmin from "../../components/menus/menu_super_admin"
import Toolbar from "../../components/utils/toolbar_super_admin"
import Footer from '../../components/utils/footer'
import api from '../../service/api'
class HomeAdmin extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      token: "",
      summaryUser: {}
    }
    this.state.token = localStorage.getItem("token");
    if (this.state.token == "") {
      window.location.href = '/';
    }
    document.title = "Home"
  }
  componentDidMount() {
    const response_graph = api.get("/api/v1/user/userstatistics", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response_graph.then(result => console.log(result.data));

    const response2 = api.get("/api/v1/user/userstatistics", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response2.then(result => this.setState({ summaryUser: result.data }));


  }
  render() {


    return (
      <div className="container-xxl position-relative bg-white d-flex p-0">
        <MenuSuperAdmin />
        {/* Content Start */}
        <div className="content">
          <Toolbar />

          <div className="container-fluid pt-4 px-4">
            <div className="row g-4">
              <div className="col-sm-12 col-xl-12">
                <img src="img/truck3.png" alt="" style={{ width: '60%', display: "block", marginLeft: "auto", marginRight: "auto" }} />
              </div>
            </div>
          </div>
          <div className="container-fluid pt-4 px-4">
            <div className="row g-4">
              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-id-card fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-2" style={{ textAlign: " center", fontSize: "18px" }}>Condutores</p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{this.state.summaryUser.drivers}</h6>
                  </div>
                </div>
              </div>
              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-user fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-2" style={{ textAlign: " center", fontSize: "18px" }}>Managers</p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{this.state.summaryUser.managers}</h6>
                  </div>
                </div>
              </div>
              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-users fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-2" style={{ textAlign: " center", fontSize: "18px" }}>Clientes</p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{this.state.summaryUser.clients}</h6>
                  </div>
                </div>
              </div>
              <div className="col-sm-6 col-xl-3">
                <div className="bg-light rounded d-flex align-items-center justify-content-between p-4">
                  <i className="fa fa-database fa-3x text-primary" />
                  <div className="ms-3">
                    <p className="mb-2" style={{ textAlign: " center", fontSize: "18px" }}>Total </p>
                    <h6 className="mb-0" style={{ textAlign: " center", fontSize: "20px" }}>{this.state.summaryUser.total}</h6>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Widgets End */}
          {/* Footer Start */}
          <Footer />
          {/* Footer End */}
        </div>
      </div>
    )
  }
}


export default HomeAdmin

