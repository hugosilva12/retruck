import React, { useRef } from 'react';
import MenuSuperAdmin from "../../components/menus/menu_super_admin"
import Toolbar from "../../components/utils/toolbar_super_admin"
import api from '../../service/api';
import Footer from '../../components/utils/footer';
import emailjs from '@emailjs/browser';
import { TruckCategory } from '../../global/truck_category'
class AddUser extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      value: 0,
      token: "",
      username: "",
      password: "",
      name: "",
      profile: -1,
      organization_id: "",
      organizations: [],
      error: "",
      license: -1,
      showAlert: false,
      showLicence: false,
      email: "",
      photofilename: "",
      imagesrc: process.env.REACT_APP_PHOTOPATH + "anonymous.png",
    }

    this.state.token = localStorage.getItem("token");

    if (this.state.token == "") {
      window.location.href = '/';
    }
    document.title = "User"

  }

  photofilename = "anonymous.png";
  imagesrc = process.env.REACT_APP_PHOTOPATH + this.photofilename;


  sendEmail = (e) => {
    e.preventDefault();

    emailjs.sendForm('service_ea972r4', 'template_7rlk85v', e.target, 'VnlvR3YOZ4tfBgLED')
      .then((result) => {
        alert('Enviado com sucesso!');
        window.location.reload(false);
      }, (error) => {
        alert('Algo falhou!');
      });
 
  };

  //Upload-Photos
  handleFileSelected(event) {
    event.preventDefault();

    const formData = new FormData();
    formData.append(
      "myFile",
      event.target.files[0],
      event.target.files[0].name
    );

    fetch('https://localhost:7226/api/v1/photo/save-file', {
      method: 'POST',
      body: formData
    })
      .then(res => res.json())
      .then((result) => {
      },
        (error) => {
          alert('Falha no upload de foto');
        })
  }


  //Selects
  handleChangeProfile = (e) => {
    this.setState({ profile: e.target.value });
    if (e.target.value == 0) {
      this.setState({ showLicence: true });
    } else {
      this.setState({ showLicence: false });
    }
  }


  handleChangeLicense = (e) => {
    this.setState({ license: e.target.value });

  }

  handleChangeOrganization = (e) => {
    this.setState({ organization_id: e.target.value });

  }



  componentDidMount() {

    const response = api.get("/api/v1/organization", {
      headers: { Authorization: `Bearer ${this.state.token}` }
    });

    response.then(result => this.setState({ organizations: result.data }));
  }

  handlePost = async e => {
    e.preventDefault();
    let isToPost = true
    const { username, password, name, license, organization_id, email, profile } = this.state;
    if (organization_id == "") {
      this.setState({ error: "Atribua Organização!" });
      isToPost = false
    }
    if (profile == -1) {
      this.setState({ error: "Atribua Perfil!" });
      isToPost = false
    }


    if (isToPost) {
      const response = await api.post("/api/v1/user", JSON.stringify({
        username: username,
        password: password,
        name: name,
        organizationId: organization_id,
        role: profile,
        photofilename: process.env.REACT_APP_PHOTOPATH,
        category: license,
        email: email

      }), {
        headers: {
          Authorization: `Bearer ${this.state.token}`, 'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      });
      if (response.status == 200) {
        this.setState({ error: "Criado com sucesso!", showAlert: true });

      } else {
        this.setState({ error: "Campos Inválidos" });
      }
    }

    alert(`Criado com sucesso !`);
  
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
              <h1 >Criar Utilizador</h1>
            </div>
            <div className="row g-4">
              <div className="col-sm-12 col-xl-12">
                <div className="bg-light rounded h-100 p-4">
                  <img src="img/add-user.png" style={{ height: 50 }} />
                  <form>
                    <div className="mb-3">
                      <label htmlFor="exampleInputEmail1" className="form-label" >Username</label>
                      <input type="text" className="form-control" required id="text" onChange={e => this.setState({ username: e.target.value })} />
                    </div>
                    <div className="mb-3">
                      <label htmlFor="exampleInputEmail1" className="form-label" >Email</label>
                      <input type="email" className="form-control" required id="text" onChange={e => this.setState({ email: e.target.value })} />
                    </div>
                    <div className="mb-3">
                      <label htmlFor="exampleInputEmail1" className="form-label" >Nome</label>
                      <input type="text" className="form-control" required id="text" onChange={e => this.setState({ name: e.target.value })} />
                    </div>
                    <div className="mb-3">
                      <label htmlFor="exampleInputPassword1" className="form-label" >Password</label>
                      <input type="password" required className="form-control" id="exampleInputPassword1" onChange={e => this.setState({ password: e.target.value })} />
                    </div>
                    <div className="form-floating mb-3">
                      <select onChange={this.handleChangeOrganization} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                        <option selected> </option>
                        {this.state.organizations.map((organization) => (
                          <option value={organization.id}>{organization.name}</option>
                        ))}
                      </select>
                      <label htmlFor="floatingSelect">Organização</label>
                    </div>
                    <div className="form-floating mb-3">
                      <select onChange={this.handleChangeProfile} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                        <option selected></option>
                        <option value={1}>Manager</option>
                        <option value={0}>Driver</option>
                        <option value={2}>Client</option>
                      </select>
                      <label htmlFor="floatingSelect">Perfil de Utilizador</label>
                    </div>
                    {this.state.showLicence == true &&
                      <>
                        <div className="form-floating mb-3">
                          <select onChange={this.handleChangeLicense} className="form-select" id="floatingSelect" aria-label="Floating label select example">
                            <option selected></option>
                            {TruckCategory.map((truck, index) => (
                              <>
                                <option value={index}>{truck.category}</option>

                              </>
                            ))}
                          </select>
                          <label htmlFor="floatingSelect">Tipo de Licença</label>
                        </div>

                      </>
                    }
                    <div className="form-floating mb-3">
                      <input onChange={this.handleFileSelected} type="File" />
                    </div>
                    <button className="btn btn-primary" onClick={this.handlePost} style={{ width: 140 }}>Criar User</button>
                    <h3 class="text-center mb-0">{this.state.error}</h3>

                  </form>
                  {this.state.showAlert == true &&
                    <>
                      <form onSubmit={this.sendEmail}>
                        <div className="col-4">
                          <input
                            type="hidden"
                            className="form-control"
                            name="email"
                            placeholder="email@example.com"
                            defaultValue={this.state.email}
                          />
                          <input
                            type="hidden"
                            className="form-control"
                            name="name"
                            defaultValue={this.state.name}
                          />
                          <input
                            type="hidden"
                            className="form-control"
                            name="username"
                            defaultValue={this.state.username}
                          />
                          <input
                            type="hidden"
                            className="form-control"
                            name="password"
                            defaultValue={this.state.password}
                          />
                        </div>
                        <div className="mb-3 col-2 ">
                          <input
                            type="submit"
                            className="btn btn-primary"
                            value="Enviar Email"
                          ></input>
                        </div>
                      </form>
                    </>
                  }
                </div>
              </div>

            </div>
          </div>
          {/* Form End */}
          {/* Footer Start */}
          <Footer />
          {/* Footer End */}
        </div>
        {/* Content End */}
      </div>
    )
  }
}


export default AddUser

