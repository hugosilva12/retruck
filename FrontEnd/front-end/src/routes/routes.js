import React from 'react';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom"
import NotFound from '../pages/global/not_found'
import Login from '../pages/global/login'
import HomeAdmin from '../pages/super_admin/component_home'
import AddOrganization from '../pages/super_admin/component_add_organizations'
import ListOrganization from '../pages/super_admin/component_list_organization'
import UpdateOrganization from '../pages/super_admin/component_update_organizations'
import UpdateUser from '../pages/super_admin/component_update_user'
import ListCustomers from '../pages/super_admin/component_list_customer'
import ListEmployee from '../pages/super_admin/component_list_employees'
import AddUser from '../pages/super_admin/component_add_user'
import HomeManager from '../pages/manager/component_home'
import AbsenceList from '../pages/manager/absence/component_absence_list'
import AbsencePending from '../pages/manager/absence/component_absence_pending'
import AddTruck from '../pages/manager/trucks/component_add_truck'
import ListTruck from '../pages/manager/trucks/component_list_trucks'
import TruckDetails from '../pages/manager/trucks/component_truck_details'
import AddRevision from '../pages/manager/trucks/component_revision_add'
import RevisionList from '../pages/manager/trucks/component_revision_list'
import UpdateRevision from '../pages/manager/trucks/componente_revision_update'
import TransportList from '../pages/manager/transports/component_transports_list'
import TransportPending from '../pages/manager/transports/component_transports_pending'
import TransportReviewParameters from '../pages/manager/transports/config/component_transport_review_parameters'
import TransportAnalysis from '../pages/manager/transports/component_transport_analysis'
import ServicesList from '../pages/manager/transports/component_services_list'
import ServicesDetails from '../pages/manager/transports/component_services_details'

function Routing() {

  return (
    <>
      <Router>
        <Routes>
          <Route path="/" element={< Login />} />
          <Route path="*" element={< NotFound />} />
          {/* Routes SuperAdmin */}
          <Route exact path='/home' element={< HomeAdmin />} />
          <Route exact path='/addorganization' element={< AddOrganization />} />
          <Route exact path='/adduser' element={< AddUser />} />
          <Route exact path='/listorganization' element={<ListOrganization />} />
          <Route exact path='/organization/:id' element={< UpdateOrganization />} />
          <Route exact path='/user/:id' element={< UpdateUser />} />
          <Route exact path='/listcustomer' element={< ListCustomers />} />
          <Route exact path='/listemployee' element={< ListEmployee />} />
          {/* Routes Manager */}
          <Route exact path='/homemanager' element={< HomeManager />} />
          <Route exact path='/absence' element={< AbsenceList />} />
          <Route exact path='/absencepending' element={< AbsencePending />} />
          <Route exact path='/addtruck' element={< AddTruck />} />
          <Route exact path='/listtruck' element={< ListTruck />} />
          <Route exact path='/truck/:id' element={< TruckDetails />} />
          <Route exact path='/addrevision' element={< AddRevision />} />
          <Route exact path='/listrevision' element={< RevisionList />} />
          <Route exact path='/revision/:id' element={< UpdateRevision />} />
          <Route exact path='/listtransport' element={< TransportList />} />
          <Route exact path='/transportpending' element={< TransportPending />} />
          <Route exact path='/config' element={< TransportReviewParameters />} />
          <Route exact path='/transport/:id' element={< TransportAnalysis />} />
          <Route exact path='/services' element={< ServicesList />} />
          <Route exact path='/services/:id' element={< ServicesDetails />} />
        </Routes>
      </Router>
    </>
  );
}

export default Routing;